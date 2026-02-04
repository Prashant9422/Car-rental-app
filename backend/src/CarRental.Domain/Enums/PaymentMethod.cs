namespace CarRental.Domain.Enums;

/// <summary>
/// Represents the method of payment.
/// </summary>
public enum PaymentMethod
{
    CreditCard = 0,
    DebitCard = 1,
    Cash = 2,
    BankTransfer = 3,
    PayPal = 4,
    ApplePay = 5,
    GooglePay = 6
}
