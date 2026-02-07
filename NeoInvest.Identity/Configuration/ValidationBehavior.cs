using FluentValidation;
using MediatR;

namespace NeoInvest.Identity.Configuration;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		if (validators is null || !validators.Any())
		{
			return await next(cancellationToken);
		}

		var context = new ValidationContext<TRequest>(request);
		var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

		var errors = validationResults
			.SelectMany(result => result.Errors)
			.Where(failure => failure != null)
			.ToList();

		if (errors.Count != 0)
		{
			throw new ValidationException(errors);
		}

		return await next(cancellationToken);
	}
}
