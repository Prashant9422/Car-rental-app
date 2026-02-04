using CarRental.Domain.Entities;

namespace CarRental.Application.Interfaces;

/// <summary>
/// Service interface for JWT token operations.
/// </summary>
public interface IJwtTokenService
{
    string GenerateToken(User user);
    (bool IsValid, Guid UserId, string Email) ValidateToken(string token);
}
