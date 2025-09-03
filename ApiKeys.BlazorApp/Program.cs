using ApiKeys.BlazorApp.Components;
using ApiKeys.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add HTTP client for API testing
builder.Services.AddHttpClient();

// Add controllers for API endpoints
builder.Services.AddControllers();

// Add Entity Framework with SQLite
builder.Services.AddDbContext<ApiKeyDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=apikeys.db"));

// Add ApiKey services
builder.Services.AddScoped<SqliteApiKeyManager>();
builder.Services.AddScoped<ApiKeyManager>(provider => provider.GetRequiredService<SqliteApiKeyManager>());

// Add HTTP context accessor for authorization handler
builder.Services.AddHttpContextAccessor();

// Add authorization services
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiKey", policy =>
        policy.Requirements.Add(new ApiKeyRequirement()));
});

builder.Services.AddScoped<IAuthorizationHandler, ApiKeyAuthorizationHandler>();

// Add Radzen services
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

var app = builder.Build();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApiKeyDbContext>();
    await context.Database.EnsureCreatedAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

// Add authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map API controllers
app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await app.RunAsync();
