using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace NeoInvest.Identity.Feature.Users.LoginUser;

public class LoginUserValidator : AbstractValidator<LoginUserCommand>
{
	public LoginUserValidator()
	{
		RuleFor(x => x.Email).NotEmpty().EmailAddress();
		RuleFor(x => x.Password).NotEmpty();
	}
}