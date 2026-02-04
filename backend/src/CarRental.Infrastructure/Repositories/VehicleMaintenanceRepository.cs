using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using CarRental.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for VehicleMaintenance entity.
/// </summary>
public class VehicleMaintenanceRepository : Repository<VehicleMaintenance>, IVehicleMaintenanceRepository
{
    public VehicleMaintenanceRepository(CarRentalDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<VehicleMaintenance>> GetMaintenanceByCarAsync(Guid carId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(m => m.CarId == carId)
            .OrderByDescending(m => m.ServiceDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<VehicleMaintenance>> GetUpcomingMaintenanceAsync(CancellationToken cancellationToken = default)
    {
        var thirtyDaysFromNow = DateTime.UtcNow.AddDays(30);
        return await _dbSet
            .Include(m => m.Car)
            .Where(m => m.NextServiceDue != null && m.NextServiceDue <= thirtyDaysFromNow)
            .OrderBy(m => m.NextServiceDue)
            .ToListAsync(cancellationToken);
    }

    public async Task<VehicleMaintenance?> GetLatestMaintenanceByCarAsync(Guid carId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(m => m.CarId == carId)
            .OrderByDescending(m => m.ServiceDate)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
