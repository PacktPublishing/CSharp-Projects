using System.Security.Cryptography;
using System.Text;

namespace CardTrackerWebApi.Services;

public class HmacHashingService : IHashingService
{
    public byte[] ComputeHash(string inputString, byte[] salt)
    {
        using HashAlgorithm hmac = new HMACSHA512();
        
        byte[] saltedInput = new byte[salt.Length + Encoding.UTF8.GetByteCount(inputString)];
        
        return hmac.ComputeHash(saltedInput);
    }

    public byte[] GenerateSalt()
    {
        byte[] salt = new byte[16];
        
        using RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        
        return salt;
    }
}