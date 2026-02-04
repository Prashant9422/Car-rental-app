using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Application.DTOs.Auth;

/// <summary>
/// DTO for user registration request.
/// </summary>
public class RegisterRequest
{
    /// <summary>User's email address</summary>
    /// <example>john.doe@email.com</example>
    [DefaultValue("john.doe@email.com")]
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>Password (min 8 characters, require uppercase, lowercase, and number)</summary>
    /// <example>SecurePass123!</example>
    [DefaultValue("SecurePass123!")]
    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;

    /// <summary>Confirm password (must match password)</summary>
    /// <example>SecurePass123!</example>
    [DefaultValue("SecurePass123!")]
    [Required]
    public string ConfirmPassword { get; set; } = string.Empty;

    /// <summary>User's first name</summary>
    /// <example>John</example>
    [DefaultValue("John")]
    [Required]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>User's last name</summary>
    /// <example>Doe</example>
    [DefaultValue("Doe")]
    [Required]
    public string LastName { get; set; } = string.Empty;

    /// <summary>User's phone number</summary>
    /// <example>+1-555-123-4567</example>
    [DefaultValue("+1-555-123-4567")]
    [Required]
    public string PhoneNumber { get; set; } = string.Empty;
}
