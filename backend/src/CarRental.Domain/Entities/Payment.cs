using CarRental.Domain.Common;
using CarRental.Domain.Enums;

namespace CarRental.Domain.Entities;

/// <summary>
/// Represents a payment for a rental.
/// </summary>
public class Payment : BaseEntity
{
    public Guid RentalId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public string? TransactionId { get; set; }
    public string? Description { get; set; }
    public string? FailureReason { get; set; }

    // Navigation property
    public virtual Rental Rental { get; set; } = null!;
}
