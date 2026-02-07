using MediatR;

namespace NeoInvest.Identity.Feature.Users.RegisterUser;

public class RegisterUserCommand : IRequest<Result<string>>
{
	public required string Email { get; init; }
	public required string Password { get; init; }
	public required string FirstName { get; init; }
	public required string LastName { get; init; }
	public ICollection<string>? Roles { get; init; }
}
