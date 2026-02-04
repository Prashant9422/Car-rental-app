using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CarRental.Infrastructure.Services;

/// <summary>
/// JWT token service implementation.
/// </summary>
public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;
    private readonly string _key;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expirationHours;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
        _key = _configuration["Jwt:Key"] ?? "CarRentalSecretKey2024!@#$%^&*()_+";
        _issuer = _configuration["Jwt:Issuer"] ?? "CarRentalAPI";
        _audience = _configuration["Jwt:Audience"] ?? "CarRentalClients";
        _expirationHours = int.Parse(_configuration["Jwt:ExpirationHours"] ?? "24");
    }

    public string GenerateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(_expirationHours),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public (bool IsValid, Guid UserId, string Email) ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_key);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            
            var userIdClaim = principal.FindFirst(JwtRegisteredClaimNames.Sub);
            var emailClaim = principal.FindFirst(JwtRegisteredClaimNames.Email);

            if (userIdClaim != null && emailClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return (true, userId, emailClaim.Value);
            }
        }
        catch
        {
            // Token validation failed
        }

        return (false, Guid.Empty, string.Empty);
    }
}
