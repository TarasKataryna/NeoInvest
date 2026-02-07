using FluentValidation;

namespace NeoInvest.Identity.Feature.Users.RegisterUser;

public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
{
	public RegisterUserValidator()
	{
		RuleFor(x => x.Email).NotEmpty().EmailAddress();
		RuleFor(x => x.Password)
			.NotEmpty()
			.MinimumLength(6)
			.Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
			.Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
			.Matches("[0-9]").WithMessage("Password must contain at least one digit.");
		RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
		RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
	}
}
