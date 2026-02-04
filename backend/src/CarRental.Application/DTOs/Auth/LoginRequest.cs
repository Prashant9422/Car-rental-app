using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Application.DTOs.Auth;

/// <summary>
/// DTO for login request.
/// </summary>
public class LoginRequest
{
    /// <summary>User's email address</summary>
    /// <example>admin@carrental.com</example>
    [DefaultValue("admin@carrental.com")]
    [Required]
    public string Email { get; set; } = string.Empty;

    /// <summary>User's password</summary>
    /// <example>Admin@123</example>
    [DefaultValue("Admin@123")]
    [Required]
    public string Password { get; set; } = string.Empty;
}
