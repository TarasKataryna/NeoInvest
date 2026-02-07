using MediatR;

namespace NeoInvest.Identity.Feature.Users.LoginUser;

public class LoginUserCommand : IRequest<Result<string>>
{
	public string Email { get; init; } = null!;
	public string Password { get; init; } = null!;
}
