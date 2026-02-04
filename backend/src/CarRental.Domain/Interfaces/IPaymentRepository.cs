using CarRental.Domain.Entities;
using CarRental.Domain.Enums;

namespace CarRental.Domain.Interfaces;

/// <summary>
/// Repository interface for Payment-specific operations.
/// </summary>
public interface IPaymentRepository : IRepository<Payment>
{
    Task<IEnumerable<Payment>> GetPaymentsByRentalAsync(Guid rentalId, CancellationToken cancellationToken = default);
    Task<decimal> GetTotalPaidForRentalAsync(Guid rentalId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(PaymentStatus status, CancellationToken cancellationToken = default);
}
