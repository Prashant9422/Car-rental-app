using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CarRental.Domain.Enums;

namespace CarRental.Application.DTOs.Payment;

/// <summary>
/// DTO for creating a new payment.
/// </summary>
public class CreatePaymentRequest
{
    /// <summary>ID of the rental to pay for</summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    [Required]
    public Guid RentalId { get; set; }

    /// <summary>Payment amount in USD</summary>
    /// <example>150.00</example>
    [DefaultValue(150.00)]
    [Required]
    public decimal Amount { get; set; }

    /// <summary>Payment method (0=CreditCard, 1=DebitCard, 2=Cash, 3=BankTransfer, 4=PayPal)</summary>
    /// <example>0</example>
    [Required]
    public PaymentMethod PaymentMethod { get; set; }

    /// <summary>Optional payment description or notes</summary>
    /// <example>Initial rental payment</example>
    [DefaultValue("Initial rental payment")]
    public string? Description { get; set; }
}
