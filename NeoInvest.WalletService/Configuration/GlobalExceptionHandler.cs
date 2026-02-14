using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace WalletService.Configuration;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken ct)
	{
		logger.LogError(exception, "An unhandled exception occurred: {Message}.", exception.Message);

		var (statusCode, message) = exception switch
		{
			ValidationException => (StatusCodes.Status400BadRequest, "Validation Error"),
			KeyNotFoundException => (StatusCodes.Status404NotFound, exception.Message),
			_ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
		};

		var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
		{
			Status = statusCode,
			Title = message,
			Detail = exception.Message
		};

		if (exception is ValidationException validationException)
		{
			problemDetails.Extensions["errors"] = validationException.Errors
				.GroupBy(e => e.PropertyName)
				.ToDictionary(
					g => g.Key,
					g => g.Select(e => e.ErrorMessage).ToArray()
				);
		}

		httpContext.Response.StatusCode = statusCode;
		await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: ct);

		return true;
	}
}
