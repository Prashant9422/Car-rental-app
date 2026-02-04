using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Application.DTOs.User;

/// <summary>
/// DTO for updating a user profile.
/// </summary>
public class UpdateUserRequest
{
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

    /// <summary>Street address</summary>
    /// <example>123 Main Street</example>
    [DefaultValue("123 Main Street")]
    public string? Street { get; set; }

    /// <summary>City</summary>
    /// <example>New York</example>
    [DefaultValue("New York")]
    public string? City { get; set; }

    /// <summary>State or Province</summary>
    /// <example>NY</example>
    [DefaultValue("NY")]
    public string? State { get; set; }

    /// <summary>Postal/ZIP code</summary>
    /// <example>10001</example>
    [DefaultValue("10001")]
    public string? PostalCode { get; set; }

    /// <summary>Country</summary>
    /// <example>USA</example>
    [DefaultValue("USA")]
    public string? Country { get; set; }

    /// <summary>Driver's license number</summary>
    /// <example>DL123456789</example>
    [DefaultValue("DL123456789")]
    public string? DriversLicenseNumber { get; set; }

    /// <summary>Driver's license expiry date</summary>
    /// <example>2028-12-31</example>
    public DateTime? DriversLicenseExpiry { get; set; }

    /// <summary>Date of birth</summary>
    /// <example>1990-05-15</example>
    public DateTime? DateOfBirth { get; set; }
}
