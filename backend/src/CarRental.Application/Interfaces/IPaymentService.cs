using CarRental.Application.Common;
using CarRental.Application.DTOs.Payment;

namespace CarRental.Application.Interfaces;

/// <summary>
/// Service interface for payment operations.
/// </summary>
public interface IPaymentService
{
    Task<Result<PaymentDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<PaymentDto>>> GetByRentalAsync(Guid rentalId, CancellationToken cancellationToken = default);
    Task<Result<PaymentDto>> CreateAsync(CreatePaymentRequest request, CancellationToken cancellationToken = default);
    Task<Result<PaymentDto>> ProcessAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<PaymentDto>> RefundAsync(Guid id, decimal? amount = null, CancellationToken cancellationToken = default);
}
