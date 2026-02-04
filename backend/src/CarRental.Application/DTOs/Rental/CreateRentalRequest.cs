using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Application.DTOs.Rental;

/// <summary>
/// DTO for creating a new rental.
/// </summary>
public class CreateRentalRequest
{
    /// <summary>ID of the car to rent</summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    [Required]
    public Guid CarId { get; set; }

    /// <summary>ID of the customer</summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    [Required]
    public Guid CustomerId { get; set; }

    /// <summary>Rental start date</summary>
    /// <example>2026-02-01</example>
    [Required]
    public DateTime StartDate { get; set; }

    /// <summary>Rental end date</summary>
    /// <example>2026-02-07</example>
    [Required]
    public DateTime EndDate { get; set; }

    /// <summary>Pickup location address</summary>
    /// <example>123 Main Street, Downtown</example>
    [DefaultValue("123 Main Street, Downtown")]
    public string? PickupLocation { get; set; }

    /// <summary>Return location address</summary>
    /// <example>456 Airport Road, Terminal 1</example>
    [DefaultValue("456 Airport Road, Terminal 1")]
    public string? ReturnLocation { get; set; }

    /// <summary>Additional notes</summary>
    /// <example>Please have the car ready by 10 AM</example>
    public string? Notes { get; set; }
}
