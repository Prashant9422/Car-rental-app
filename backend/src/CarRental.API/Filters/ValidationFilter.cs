using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CarRental.API.Filters;

/// <summary>
/// Action filter for validating requests using FluentValidation.
/// </summary>
public class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            context.Result = new BadRequestObjectResult(new
            {
                success = false,
                statusCode = 400,
                errorCode = "VALIDATION_ERROR",
                message = "One or more validation errors occurred.",
                errors = errors,
                timestamp = DateTime.UtcNow
            });

            return;
        }

        await next();
    }
}
