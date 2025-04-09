using System.Text;
using CardTrackerWebApi.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace CardTrackerWebApi.Helpers;

public static class AuthExtensions
{
    public static void AddJwtAuthentication(this IServiceCollection services, AuthSettings jwtSettings)
    {
        if (string.IsNullOrWhiteSpace(jwtSettings.Audience) || 
            string.IsNullOrWhiteSpace(jwtSettings.Issuer) ||
            string.IsNullOrWhiteSpace(jwtSettings.Secret))
        {
            throw new ArgumentException("Auth settings are not configured properly.", nameof(jwtSettings));
        }
        
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
            });
    }

    public static string GetCurrentUsername(this HttpContext httpContext)
    {
        return httpContext.User.Identity?.Name
               ?? throw new InvalidOperationException("User is not authenticated.");
    }
}