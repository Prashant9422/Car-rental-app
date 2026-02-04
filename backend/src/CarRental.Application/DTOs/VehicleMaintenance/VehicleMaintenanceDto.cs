using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Application.DTOs.VehicleMaintenance;

/// <summary>
/// DTO for vehicle maintenance data returned in responses.
/// </summary>
public class VehicleMaintenanceDto
{
    public Guid Id { get; set; }
    public Guid CarId { get; set; }
    public string CarInfo { get; set; } = string.Empty;
    public DateTime ServiceDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string ServiceType { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public int? OdometerReading { get; set; }
    public DateTime? NextServiceDue { get; set; }
    public string? ServiceProvider { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Request DTO for creating a new maintenance record.
/// </summary>
public class CreateMaintenanceRequest
{
    /// <summary>ID of the car being serviced</summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    [Required]
    public Guid CarId { get; set; }

    /// <summary>Date of service</summary>
    /// <example>2026-01-29</example>
    [Required]
    public DateTime ServiceDate { get; set; }

    /// <summary>Detailed description of the maintenance work</summary>
    /// <example>Full oil change with synthetic 5W-30, replaced oil filter</example>
    [DefaultValue("Full oil change with synthetic 5W-30, replaced oil filter")]
    [Required]
    public string Description { get; set; } = string.Empty;

    /// <summary>Type of service performed</summary>
    /// <example>Oil Change</example>
    [DefaultValue("Oil Change")]
    [Required]
    public string ServiceType { get; set; } = string.Empty;

    /// <summary>Total cost of the maintenance in USD</summary>
    /// <example>89.99</example>
    [DefaultValue(89.99)]
    [Required]
    public decimal Cost { get; set; }

    /// <summary>Odometer reading at time of service</summary>
    /// <example>45000</example>
    [DefaultValue(45000)]
    public int? OdometerReading { get; set; }

    /// <summary>Date when next service is due</summary>
    /// <example>2026-04-29</example>
    public DateTime? NextServiceDue { get; set; }

    /// <summary>Name of the service provider/mechanic</summary>
    /// <example>Quick Lube Auto Center</example>
    [DefaultValue("Quick Lube Auto Center")]
    public string? ServiceProvider { get; set; }

    /// <summary>Additional notes about the service</summary>
    /// <example>Brake pads at 60% wear, recommend replacement at next service</example>
    public string? Notes { get; set; }
}
