using CarRental.Domain.Entities;
using CarRental.Domain.Enums;
using CarRental.Domain.Interfaces;
using CarRental.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Repositories;

/// <summary>
/// Rental repository implementation.
/// </summary>
public class RentalRepository : Repository<Rental>, IRentalRepository
{
    public RentalRepository(CarRentalDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Rental>> GetRentalsByCustomerAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.Car)
            .Include(r => r.Customer)
            .Where(r => r.CustomerId == customerId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Rental>> GetRentalsByCarAsync(Guid carId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.Car)
            .Include(r => r.Customer)
            .Where(r => r.CarId == carId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Rental>> GetActiveRentalsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.Car)
            .Include(r => r.Customer)
            .Where(r => r.Status == RentalStatus.Active)
            .OrderByDescending(r => r.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Rental>> GetRentalsByStatusAsync(RentalStatus status, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.Car)
            .Include(r => r.Customer)
            .Where(r => r.Status == status)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Rental>> GetOverdueRentalsAsync(CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;
        return await _dbSet
            .Include(r => r.Car)
            .Include(r => r.Customer)
            .Where(r => r.Status == RentalStatus.Active && r.EndDate < today)
            .OrderBy(r => r.EndDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Rental?> GetRentalWithDetailsAsync(Guid rentalId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.Car)
            .Include(r => r.Customer)
            .Include(r => r.Payments)
            .FirstOrDefaultAsync(r => r.Id == rentalId, cancellationToken);
    }

    public async Task<bool> HasOverlappingRentalAsync(Guid carId, DateTime startDate, DateTime endDate, Guid? excludeRentalId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Where(r => r.CarId == carId)
            .Where(r => r.Status != RentalStatus.Cancelled && r.Status != RentalStatus.Completed)
            .Where(r => r.StartDate <= endDate && r.EndDate >= startDate);

        if (excludeRentalId.HasValue)
            query = query.Where(r => r.Id != excludeRentalId.Value);

        return await query.AnyAsync(cancellationToken);
    }
}
