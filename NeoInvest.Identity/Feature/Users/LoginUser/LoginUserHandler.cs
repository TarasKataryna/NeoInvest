using MediatR;
using Microsoft.AspNetCore.Identity;
using NeoInvest.Identity.Entities;
using NeoInvest.Identity.Infrastructure;

namespace NeoInvest.Identity.Feature.Users.LoginUser;

public class LoginUserHandler(UserManager<User> userManager, JwtTokenService jwtTokenService) : IRequestHandler<LoginUserCommand, Result<string>>
{
	private readonly UserManager<User> _userManager = userManager;
	private readonly JwtTokenService _jwtTokenService = jwtTokenService;

	public async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken ct)
	{
		var user = _userManager.Users.FirstOrDefault(u => u.Email == request.Email);
		if (user is null)
			return Result<string>.Failure("Invalid email or password.");

		var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
		if (!passwordValid)
			return Result<string>.Failure("Invalid email or password.");

		var roleNames = await _userManager.GetRolesAsync(user);
		
		return Result<string>.Success(_jwtTokenService.GenerateToken(user, roleNames));
	}
}
