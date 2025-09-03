using Microsoft.EntityFrameworkCore;

namespace ApiKeys.Service;

public class ApiKeyRecord
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Hash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class ApiKeyDbContext : DbContext
{
    public ApiKeyDbContext(DbContextOptions<ApiKeyDbContext> options) : base(options) { }

    public DbSet<ApiKeyRecord> ApiKeys { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApiKeyRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Hash).IsRequired().HasMaxLength(500);
            entity.HasIndex(e => e.Name).IsUnique();
            entity.HasIndex(e => e.Hash);
        });
    }
}

public class SqliteApiKeyManager : ApiKeyManager
{
    private readonly ApiKeyDbContext _context;
    private readonly string _salt;

    public SqliteApiKeyManager(ApiKeyDbContext context, string salt = "default_salt_change_in_production")
    {
        _context = context;
        _salt = salt;
    }

    protected override string Salt => _salt;

    protected override async Task StoreHashAsync(string name, string hash)
    {
        var existingKey = await _context.ApiKeys.FirstOrDefaultAsync(k => k.Name == name);
        if (existingKey != null)
        {
            existingKey.Hash = hash;
            existingKey.CreatedAt = DateTime.UtcNow;
        }
        else
        {
            _context.ApiKeys.Add(new ApiKeyRecord
            {
                Name = name,
                Hash = hash,
                CreatedAt = DateTime.UtcNow
            });
        }
        await _context.SaveChangesAsync();
    }

    protected override async Task<string> GetStoredHashAsync(string base64Hash)
    {
        var record = await _context.ApiKeys.FirstOrDefaultAsync(k => k.Hash == base64Hash);
        return record?.Hash ?? string.Empty;
    }

    public async Task<List<ApiKeyRecord>> GetAllKeysAsync()
    {
        return await _context.ApiKeys.OrderBy(k => k.Name).ToListAsync();
    }

    public async Task DeleteKeyAsync(string name)
    {
        var record = await _context.ApiKeys.FirstOrDefaultAsync(k => k.Name == name);
        if (record != null)
        {
            _context.ApiKeys.Remove(record);
            await _context.SaveChangesAsync();
        }
    }
}