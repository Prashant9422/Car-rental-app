using CarRental.Application.DTOs.User;
using CarRental.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers;

/// <summary>
/// API endpoints for user management.
/// </summary>
[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Get all customers.
    /// </summary>
    [HttpGet("customers")]
    [Authorize(Roles = "Admin,Manager,Staff")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCustomers(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all customers");
        var result = await _userService.GetAllCustomersAsync(cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Get a user by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting user with ID: {UserId}", id);
        var result = await _userService.GetByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Update a user profile.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating user with ID: {UserId}", id);
        var result = await _userService.UpdateAsync(id, request, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Delete a user.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting user with ID: {UserId}", id);
        var result = await _userService.DeleteAsync(id, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Deactivate a user account.
    /// </summary>
    [HttpPost("{id:guid}/deactivate")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deactivating user with ID: {UserId}", id);
        var result = await _userService.DeactivateAsync(id, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Activate a user account.
    /// </summary>
    [HttpPost("{id:guid}/activate")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Activating user with ID: {UserId}", id);
        var result = await _userService.ActivateAsync(id, cancellationToken);
        return HandleResult(result);
    }
}
