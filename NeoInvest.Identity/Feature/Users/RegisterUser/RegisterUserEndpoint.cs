using MediatR;

namespace NeoInvest.Identity.Feature.Users.RegisterUser;

public record RegisterUserRequest
{
	public required string Email { get; init; }
	public required string Password { get; init; }
	public required string FirstName { get; init; }
	public required string LastName { get; init; }
	public ICollection<string>? Roles { get; init; }
}

public static class RegisterUserEndpoint
{
	public static void MapRegisterUser(this IEndpointRouteBuilder app)
	{
		app.MapPost("user/register", async (RegisterUserRequest request, ISender sender) =>
		{
			return await sender.Send(new RegisterUserCommand
			{
				Email = request.Email,
				Password = request.Password,
				FirstName = request.FirstName,
				LastName = request.LastName,
				Roles = request.Roles
			})
			switch
			{
				{ IsSuccess : true } success => Results.Ok(success.Value),
				_ => Results.BadRequest()
			};
		})
		.WithName("RegisterUser")
		.WithTags("Users");
	}
}
