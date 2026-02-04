using CarRental.Domain.Interfaces;
using CarRental.Infrastructure.Data;
using CarRental.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace CarRental.Infrastructure;

/// <summary>
/// Unit of Work implementation for managing transactions.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly CarRentalDbContext _context;
    private IDbContextTransaction? _transaction;

    private ICarRepository? _cars;
    private IUserRepository? _users;
    private IRentalRepository? _rentals;
    private IPaymentRepository? _payments;
    private IVehicleMaintenanceRepository? _vehicleMaintenances;

    public UnitOfWork(CarRentalDbContext context)
    {
        _context = context;
    }

    public ICarRepository Cars => _cars ??= new CarRepository(_context);
    public IUserRepository Users => _users ??= new UserRepository(_context);
    public IRentalRepository Rentals => _rentals ??= new RentalRepository(_context);
    public IPaymentRepository Payments => _payments ??= new PaymentRepository(_context);
    public IVehicleMaintenanceRepository VehicleMaintenances => _vehicleMaintenances ??= new VehicleMaintenanceRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
