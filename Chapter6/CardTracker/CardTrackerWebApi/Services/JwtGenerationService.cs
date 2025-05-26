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
        string secret = _jwtSettings.Secret ?? throw new InvalidOperationException("Secret is not set");
        byte[] keyBytes = Encoding.UTF8.GetBytes(secret);
        SymmetricSecurityKey key = new(keyBytes);
        SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

        return new JwtSecurityTokenHandler()
            .WriteToken(new JwtSecurityToken(
                issuer: _jwtSettings.Issuer ?? throw new InvalidOperationException("Issuer is not set"),
                audience: _jwtSettings.Audience ?? throw new InvalidOperationException("Audience is not set"),
                claims: [
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.CreateVersion7().ToString()),
                    new Claim(ClaimTypes.Role, role),
                    new Claim(ClaimTypes.Name, username),
                ],
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds));
    }
}