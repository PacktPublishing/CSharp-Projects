using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CardTrackerWebApi.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CardTrackerWebApi.Services;

public class JwtGenerationService(IOptionsSnapshot<AuthSettings> jwtSettings) : ITokenGenerationService
{
    private readonly AuthSettings _jwtSettings = jwtSettings.Value;

    public string GenerateToken(string username, string role)
    {
        Claim[] claims =
        [
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Role, role),
            new(ClaimTypes.Name, username),
        ];

        string secret = _jwtSettings.Secret ?? throw new InvalidOperationException("Secret is not set");
        byte[] keyBytes = Encoding.UTF8.GetBytes(secret);
        SymmetricSecurityKey key = new(keyBytes);
        SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);
        
        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer ?? throw new InvalidOperationException("Issuer is not set"),
            audience: _jwtSettings.Audience ?? throw new InvalidOperationException("Audience is not set"),
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}