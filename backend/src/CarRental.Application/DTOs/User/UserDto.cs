namespace CarRental.Application.DTOs.User;

/// <summary>
/// DTO for user data returned in responses.
/// </summary>
public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public string? DriversLicenseNumber { get; set; }
    public DateTime? DriversLicenseExpiry { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime CreatedAt { get; set; }
}
