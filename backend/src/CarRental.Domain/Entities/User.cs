using CarRental.Domain.Common;
using CarRental.Domain.Enums;

namespace CarRental.Domain.Entities;

/// <summary>
/// Represents a user in the system (customer, staff, admin).
/// </summary>
public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Customer;
    public bool IsActive { get; set; } = true;
    public DateTime? LastLoginAt { get; set; }

    // Address information
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }

    // Driver's license information
    public string? DriversLicenseNumber { get; set; }
    public DateTime? DriversLicenseExpiry { get; set; }
    public DateTime? DateOfBirth { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    // Navigation properties
    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}
