using CarRental.Application.DTOs.Rental;
using CarRental.Application.Interfaces;
using CarRental.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers;

/// <summary>
/// API endpoints for rental management.
/// </summary>
[Authorize]
public class RentalsController : BaseApiController
{
    private readonly IRentalService _rentalService;
    private readonly ILogger<RentalsController> _logger;

    public RentalsController(IRentalService rentalService, ILogger<RentalsController> logger)
    {
        _rentalService = rentalService;
        _logger = logger;
    }

    /// <summary>
    /// Get all rentals.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin,Manager,Staff")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<RentalDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all rentals");
        var result = await _rentalService.GetAllAsync(cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Get a rental by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<RentalDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting rental with ID: {RentalId}", id);
        var result = await _rentalService.GetByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Get rentals by customer ID.
    /// </summary>
    [HttpGet("customer/{customerId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<RentalDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCustomer(Guid customerId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting rentals for customer: {CustomerId}", customerId);
        var result = await _rentalService.GetByCustomerAsync(customerId, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Get rentals by status.
    /// </summary>
    [HttpGet("status/{status}")]
    [Authorize(Roles = "Admin,Manager,Staff")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<RentalDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByStatus(RentalStatus status, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting rentals with status: {Status}", status);
        var result = await _rentalService.GetByStatusAsync(status, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Get active rentals.
    /// </summary>
    [HttpGet("active")]
    [Authorize(Roles = "Admin,Manager,Staff")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<RentalDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActive(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting active rentals");
        var result = await _rentalService.GetActiveRentalsAsync(cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Get overdue rentals.
    /// </summary>
    [HttpGet("overdue")]
    [Authorize(Roles = "Admin,Manager,Staff")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<RentalDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOverdue(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting overdue rentals");
        var result = await _rentalService.GetOverdueRentalsAsync(cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Create a new rental.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<RentalDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateRentalRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new rental for car: {CarId}", request.CarId);
        var result = await _rentalService.CreateAsync(request, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Confirm a pending rental.
    /// </summary>
    [HttpPost("{id:guid}/confirm")]
    [ProducesResponseType(typeof(ApiResponse<RentalDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Confirm(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Confirming rental: {RentalId}", id);
        var result = await _rentalService.ConfirmAsync(id, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Start a confirmed rental (pickup).
    /// </summary>
    [HttpPost("{id:guid}/start")]
    [Authorize(Roles = "Admin,Manager,Staff")]
    [ProducesResponseType(typeof(ApiResponse<RentalDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Start(Guid id, [FromQuery] int? odometerAtPickup, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting rental: {RentalId}", id);
        var result = await _rentalService.StartAsync(id, odometerAtPickup, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Complete a rental (return).
    /// </summary>
    [HttpPost("{id:guid}/complete")]
    [Authorize(Roles = "Admin,Manager,Staff")]
    [ProducesResponseType(typeof(ApiResponse<RentalDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Complete(Guid id, [FromBody] CompleteRentalRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Completing rental: {RentalId}", id);
        var result = await _rentalService.CompleteAsync(id, request, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Cancel a rental.
    /// </summary>
    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(typeof(ApiResponse<RentalDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Cancelling rental: {RentalId}", id);
        var result = await _rentalService.CancelAsync(id, cancellationToken);
        return HandleResult(result);
    }
}
