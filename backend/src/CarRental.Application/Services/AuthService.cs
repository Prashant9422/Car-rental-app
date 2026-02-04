using CarRental.Application.Common;
using CarRental.Application.DTOs.Auth;
using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using CarRental.Domain.Enums;
using CarRental.Domain.Interfaces;

namespace CarRental.Application.Services;

/// <summary>
/// Implementation of authentication service.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        // Check if email already exists
        if (await _unitOfWork.Users.EmailExistsAsync(request.Email, cancellationToken))
            return Result<AuthResponse>.Failure($"Email '{request.Email}' is already registered.", "CONFLICT");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email.ToLower(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Role = UserRole.Customer,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Users.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var token = _jwtTokenService.GenerateToken(user);

        return Result<AuthResponse>.Success(new AuthResponse
        {
            Token = token,
            TokenType = "Bearer",
            ExpiresAt = DateTime.UtcNow.AddHours(24),
            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role.ToString()
        });
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(request.Email.ToLower(), cancellationToken);
        
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Result<AuthResponse>.Failure("Invalid email or password.", "UNAUTHORIZED");

        if (!user.IsActive)
            return Result<AuthResponse>.Failure("Your account has been deactivated. Please contact support.", "UNAUTHORIZED");

        user.LastLoginAt = DateTime.UtcNow;
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var token = _jwtTokenService.GenerateToken(user);

        return Result<AuthResponse>.Success(new AuthResponse
        {
            Token = token,
            TokenType = "Bearer",
            ExpiresAt = DateTime.UtcNow.AddHours(24),
            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role.ToString()
        });
    }

    public async Task<Result<AuthResponse>> RefreshTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        var (isValid, userId, email) = _jwtTokenService.ValidateToken(token);
        
        if (!isValid)
            return Result<AuthResponse>.Failure("Invalid or expired token.", "UNAUTHORIZED");

        var user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
        if (user == null || !user.IsActive)
            return Result<AuthResponse>.Failure("User not found or deactivated.", "UNAUTHORIZED");

        var newToken = _jwtTokenService.GenerateToken(user);

        return Result<AuthResponse>.Success(new AuthResponse
        {
            Token = newToken,
            TokenType = "Bearer",
            ExpiresAt = DateTime.UtcNow.AddHours(24),
            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role.ToString()
        });
    }
}
