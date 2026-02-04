using CarRental.Domain.Entities;

namespace CarRental.Domain.Interfaces;

/// <summary>
/// Repository interface for VehicleMaintenance entity.
/// </summary>
public interface IVehicleMaintenanceRepository : IRepository<VehicleMaintenance>
{
    Task<IEnumerable<VehicleMaintenance>> GetMaintenanceByCarAsync(Guid carId, CancellationToken cancellationToken = default);
    Task<IEnumerable<VehicleMaintenance>> GetUpcomingMaintenanceAsync(CancellationToken cancellationToken = default);
    Task<VehicleMaintenance?> GetLatestMaintenanceByCarAsync(Guid carId, CancellationToken cancellationToken = default);
}
