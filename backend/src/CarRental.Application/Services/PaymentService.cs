using CarRental.Application.Common;
using CarRental.Application.DTOs.Payment;
using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using CarRental.Domain.Enums;
using CarRental.Domain.Interfaces;

namespace CarRental.Application.Services;

/// <summary>
/// Implementation of payment service.
/// </summary>
public class PaymentService : IPaymentService
{
    private readonly IUnitOfWork _unitOfWork;

    public PaymentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaymentDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var payment = await _unitOfWork.Payments.GetByIdAsync(id, cancellationToken);
        if (payment == null)
            return Result<PaymentDto>.NotFound($"Payment with ID '{id}' was not found.");

        return Result<PaymentDto>.Success(MapToDto(payment));
    }

    public async Task<Result<IEnumerable<PaymentDto>>> GetByRentalAsync(Guid rentalId, CancellationToken cancellationToken = default)
    {
        var payments = await _unitOfWork.Payments.GetPaymentsByRentalAsync(rentalId, cancellationToken);
        return Result<IEnumerable<PaymentDto>>.Success(payments.Select(MapToDto));
    }

    public async Task<Result<PaymentDto>> CreateAsync(CreatePaymentRequest request, CancellationToken cancellationToken = default)
    {
        // Validate rental exists
        var rental = await _unitOfWork.Rentals.GetByIdAsync(request.RentalId, cancellationToken);
        if (rental == null)
            return Result<PaymentDto>.NotFound($"Rental with ID '{request.RentalId}' was not found.");

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            RentalId = request.RentalId,
            Amount = request.Amount,
            PaymentDate = DateTime.UtcNow,
            PaymentMethod = request.PaymentMethod,
            Status = PaymentStatus.Pending,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Payments.AddAsync(payment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<PaymentDto>.Success(MapToDto(payment));
    }

    public async Task<Result<PaymentDto>> ProcessAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var payment = await _unitOfWork.Payments.GetByIdAsync(id, cancellationToken);
        if (payment == null)
            return Result<PaymentDto>.NotFound($"Payment with ID '{id}' was not found.");

        if (payment.Status != PaymentStatus.Pending)
            return Result<PaymentDto>.Failure("Only pending payments can be processed.", "BUSINESS_RULE_VIOLATION");

        // Simulate payment processing
        payment.Status = PaymentStatus.Processing;
        payment.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Payments.Update(payment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Simulate successful payment (in production, this would call a payment gateway)
        payment.Status = PaymentStatus.Completed;
        payment.TransactionId = Guid.NewGuid().ToString("N").Substring(0, 16).ToUpper();
        payment.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Payments.Update(payment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<PaymentDto>.Success(MapToDto(payment));
    }

    public async Task<Result<PaymentDto>> RefundAsync(Guid id, decimal? amount = null, CancellationToken cancellationToken = default)
    {
        var payment = await _unitOfWork.Payments.GetByIdAsync(id, cancellationToken);
        if (payment == null)
            return Result<PaymentDto>.NotFound($"Payment with ID '{id}' was not found.");

        if (payment.Status != PaymentStatus.Completed)
            return Result<PaymentDto>.Failure("Only completed payments can be refunded.", "BUSINESS_RULE_VIOLATION");

        var refundAmount = amount ?? payment.Amount;
        if (refundAmount > payment.Amount)
            return Result<PaymentDto>.Failure("Refund amount cannot exceed the original payment amount.", "VALIDATION_ERROR");

        payment.Status = refundAmount == payment.Amount ? PaymentStatus.Refunded : PaymentStatus.PartialRefund;
        payment.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Payments.Update(payment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<PaymentDto>.Success(MapToDto(payment));
    }

    private static PaymentDto MapToDto(Payment payment)
    {
        return new PaymentDto
        {
            Id = payment.Id,
            RentalId = payment.RentalId,
            Amount = payment.Amount,
            PaymentDate = payment.PaymentDate,
            PaymentMethod = payment.PaymentMethod.ToString(),
            Status = payment.Status.ToString(),
            TransactionId = payment.TransactionId,
            Description = payment.Description,
            CreatedAt = payment.CreatedAt
        };
    }
}
