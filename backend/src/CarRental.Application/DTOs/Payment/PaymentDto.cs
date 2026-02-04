using CarRental.Domain.Enums;

namespace CarRental.Application.DTOs.Payment;

/// <summary>
/// DTO for payment data returned in responses.
/// </summary>
public class PaymentDto
{
    public Guid Id { get; set; }
    public Guid RentalId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? TransactionId { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}
