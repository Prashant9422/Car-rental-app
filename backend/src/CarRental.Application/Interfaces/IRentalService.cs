using CarRental.Application.Common;
using CarRental.Application.DTOs.Rental;
using CarRental.Domain.Enums;

namespace CarRental.Application.Interfaces;

/// <summary>
/// Service interface for rental operations.
/// </summary>
public interface IRentalService
{
    Task<Result<RentalDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<RentalDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<RentalDto>>> GetByCustomerAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<RentalDto>>> GetByStatusAsync(RentalStatus status, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<RentalDto>>> GetActiveRentalsAsync(CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<RentalDto>>> GetOverdueRentalsAsync(CancellationToken cancellationToken = default);
    Task<Result<RentalDto>> CreateAsync(CreateRentalRequest request, CancellationToken cancellationToken = default);
    Task<Result<RentalDto>> ConfirmAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<RentalDto>> StartAsync(Guid id, int? odometerAtPickup, CancellationToken cancellationToken = default);
    Task<Result<RentalDto>> CompleteAsync(Guid id, CompleteRentalRequest request, CancellationToken cancellationToken = default);
    Task<Result<RentalDto>> CancelAsync(Guid id, CancellationToken cancellationToken = default);
}
