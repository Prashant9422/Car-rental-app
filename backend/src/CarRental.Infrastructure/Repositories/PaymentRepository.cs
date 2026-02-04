using CarRental.Domain.Entities;
using CarRental.Domain.Enums;
using CarRental.Domain.Interfaces;
using CarRental.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Repositories;

/// <summary>
/// Payment repository implementation.
/// </summary>
public class PaymentRepository : Repository<Payment>, IPaymentRepository
{
    public PaymentRepository(CarRentalDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Payment>> GetPaymentsByRentalAsync(Guid rentalId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(p => p.RentalId == rentalId)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetTotalPaidForRentalAsync(Guid rentalId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(p => p.RentalId == rentalId && p.Status == PaymentStatus.Completed)
            .SumAsync(p => p.Amount, cancellationToken);
    }

    public async Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(PaymentStatus status, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(p => p.Status == status)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync(cancellationToken);
    }
}
