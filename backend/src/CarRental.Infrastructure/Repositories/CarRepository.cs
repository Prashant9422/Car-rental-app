using CarRental.Domain.Entities;
using CarRental.Domain.Enums;
using CarRental.Domain.Interfaces;
using CarRental.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Repositories;

/// <summary>
/// Car repository implementation.
/// </summary>
public class CarRepository : Repository<Car>, ICarRepository
{
    public CarRepository(CarRentalDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Car>> GetAvailableCarsAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        var rentedCarIds = await _context.Rentals
            .Where(r => r.Status != RentalStatus.Cancelled && r.Status != RentalStatus.Completed)
            .Where(r => r.StartDate <= endDate && r.EndDate >= startDate)
            .Select(r => r.CarId)
            .ToListAsync(cancellationToken);

        return await _dbSet
            .Where(c => c.Status == CarStatus.Available && !rentedCarIds.Contains(c.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Car>> GetCarsByCategoryAsync(CarCategory category, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.Category == category)
            .ToListAsync(cancellationToken);
    }

    public async Task<Car?> GetByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.LicensePlate == licensePlate, cancellationToken);
    }

    public async Task<IEnumerable<Car>> SearchCarsAsync(string? make, string? model, CarCategory? category, decimal? maxDailyRate, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(make))
            query = query.Where(c => c.Make.ToLower().Contains(make.ToLower()));

        if (!string.IsNullOrWhiteSpace(model))
            query = query.Where(c => c.Model.ToLower().Contains(model.ToLower()));

        if (category.HasValue)
            query = query.Where(c => c.Category == category.Value);

        if (maxDailyRate.HasValue)
            query = query.Where(c => c.DailyRate <= maxDailyRate.Value);

        return await query.ToListAsync(cancellationToken);
    }
}
