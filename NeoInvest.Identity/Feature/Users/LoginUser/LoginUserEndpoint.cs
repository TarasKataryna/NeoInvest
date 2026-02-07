using MediatR;

namespace NeoInvest.Identity.Feature.Users.LoginUser;

public record LoginUserRequest
{
	public string Email { get; init; } = null!;
	public string Password { get; init; } = null!;
}

public static class LoginUserEndpoint
{
	public static void MapLoginUser(this IEndpointRouteBuilder app)
	{
		app.MapPost("user/login", async (LoginUserRequest request, ISender sender) =>
		{
			return await sender.Send(new LoginUserCommand
			{
				Email = request.Email,
				Password = request.Password
			})
			switch
			{
				{ IsSuccess: true } success => Results.Ok(success.Value),
				_ => Results.BadRequest()
			};
		})
		.WithName("LoginUser")
		.WithTags("Users");
	}
}
