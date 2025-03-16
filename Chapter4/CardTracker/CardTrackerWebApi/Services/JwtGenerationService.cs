using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CardTrackerWebApi.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CardTrackerWebApi.Services;

public class JwtGenerationService(IOptionsSnapshot<AuthSettings> authSettings) : ITokenGenerationService
{
    public string GenerateToken(string username, string role)
    {
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.UTF8.GetBytes(authSettings.Value.Key);
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.Name, username), 
                new Claim(ClaimTypes.Role, role)
            ]),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }
}