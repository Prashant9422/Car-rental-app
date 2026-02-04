using CarRental.Application.DTOs.Payment;
using CarRental.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers;

/// <summary>
/// API endpoints for payment management.
/// </summary>
[Authorize]
public class PaymentsController : BaseApiController
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    /// <summary>
    /// Get a payment by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<PaymentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting payment with ID: {PaymentId}", id);
        var result = await _paymentService.GetByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Get payments for a rental.
    /// </summary>
    [HttpGet("rental/{rentalId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<PaymentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByRental(Guid rentalId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting payments for rental: {RentalId}", rentalId);
        var result = await _paymentService.GetByRentalAsync(rentalId, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Create a new payment.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<PaymentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreatePaymentRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new payment for rental: {RentalId}", request.RentalId);
        var result = await _paymentService.CreateAsync(request, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Process a pending payment.
    /// </summary>
    [HttpPost("{id:guid}/process")]
    [ProducesResponseType(typeof(ApiResponse<PaymentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Process(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing payment: {PaymentId}", id);
        var result = await _paymentService.ProcessAsync(id, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Refund a completed payment.
    /// </summary>
    [HttpPost("{id:guid}/refund")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(typeof(ApiResponse<PaymentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Refund(Guid id, [FromQuery] decimal? amount, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Refunding payment: {PaymentId}", id);
        var result = await _paymentService.RefundAsync(id, amount, cancellationToken);
        return HandleResult(result);
    }
}
