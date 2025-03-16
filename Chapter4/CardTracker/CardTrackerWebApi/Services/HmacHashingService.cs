using System.Security.Cryptography;
using System.Text;

namespace CardTrackerWebApi.Services;

public class HmacHashingService : IHashingService
{
    public byte[] ComputeHash(string inputString, byte[] salt)
    {
        using HMACSHA256 hmac = new HMACSHA256(salt);
        byte[] inputBytes = Encoding.UTF8.GetBytes(inputString);
        byte[] hashBytes = hmac.ComputeHash(inputBytes);
        
        return hashBytes;
    }

    public byte[] GenerateSalt()
    {
        byte[] salt = new byte[16];
        
        using RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        
        return salt;
    }
}