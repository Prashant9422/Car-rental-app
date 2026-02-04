using CarRental.Domain.Entities;
using CarRental.Domain.Enums;

namespace CarRental.Domain.Interfaces;

/// <summary>
/// Repository interface for Rental-specific operations.
/// </summary>
public interface IRentalRepository : IRepository<Rental>
{
    Task<IEnumerable<Rental>> GetRentalsByCustomerAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Rental>> GetRentalsByCarAsync(Guid carId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Rental>> GetActiveRentalsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Rental>> GetRentalsByStatusAsync(RentalStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<Rental>> GetOverdueRentalsAsync(CancellationToken cancellationToken = default);
    Task<Rental?> GetRentalWithDetailsAsync(Guid rentalId, CancellationToken cancellationToken = default);
    Task<bool> HasOverlappingRentalAsync(Guid carId, DateTime startDate, DateTime endDate, Guid? excludeRentalId = null, CancellationToken cancellationToken = default);
}
