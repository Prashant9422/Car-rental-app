using CarRental.Application.Common;
using CarRental.Application.DTOs.Auth;

namespace CarRental.Application.Interfaces;

/// <summary>
/// Service interface for authentication operations.
/// </summary>
public interface IAuthService
{
    Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> RefreshTokenAsync(string token, CancellationToken cancellationToken = default);
}
