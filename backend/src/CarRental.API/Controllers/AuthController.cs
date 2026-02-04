using CarRental.Application.DTOs.Auth;
using CarRental.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers;

/// <summary>
/// API endpoints for authentication.
/// </summary>
public class AuthController : BaseApiController
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Register a new user.
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("User registration attempt for email: {Email}", request.Email);
        var result = await _authService.RegisterAsync(request, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Login with email and password.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Login attempt for email: {Email}", request.Email);
        var result = await _authService.LoginAsync(request, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Refresh the authentication token.
    /// </summary>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Token refresh attempt");
        var result = await _authService.RefreshTokenAsync(request.Token, cancellationToken);
        return HandleResult(result);
    }
}

/// <summary>
/// DTO for refresh token request.
/// </summary>
public class RefreshTokenRequest
{
    public string Token { get; set; } = string.Empty;
}
