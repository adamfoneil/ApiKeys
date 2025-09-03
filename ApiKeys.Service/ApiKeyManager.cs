using System.Security.Cryptography;
using System.Text;

namespace ApiKeys.Service;

public abstract class ApiKeyManager
{
	protected abstract Task StoreHashAsync(string name, string hash);
	protected abstract Task<string> GetStoredHashAsync(string base64Hash);
	protected abstract string Salt { get; }

	public async Task<string> GenerateAsync(string name)
	{
		var keyBytes = new byte[32];
		RandomNumberGenerator.Fill(keyBytes);
		var apiKey = Convert.ToBase64String(keyBytes);

		// Hash the key
		var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(Salt + apiKey));
		var hash = Convert.ToBase64String(hashBytes);
		
		await StoreHashAsync(name, hash);

		// client should see only once and store it securely
		return apiKey;
	}

	public async Task<bool> ValidateAsync(string apiKey)
	{
		var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(Salt + apiKey));
		var hash = Convert.ToBase64String(hashBytes);
		var storedHash = await GetStoredHashAsync(hash);
		return storedHash == hash;
	}
}
