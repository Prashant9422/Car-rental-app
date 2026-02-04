namespace CarRental.Domain.Interfaces;

/// <summary>
/// Unit of Work interface for managing transactions across repositories.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    ICarRepository Cars { get; }
    IUserRepository Users { get; }
    IRentalRepository Rentals { get; }
    IPaymentRepository Payments { get; }
    IVehicleMaintenanceRepository VehicleMaintenances { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
