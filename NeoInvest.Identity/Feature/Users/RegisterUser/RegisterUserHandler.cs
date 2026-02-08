using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NeoInvest.Identity.Entities;
using NeoInvest.Identity.Infrastructure;
using NeoInvest.Shared.Events;

namespace NeoInvest.Identity.Feature.Users.RegisterUser;

public class RegisterUserHandler(
	UserManager<User> userManager, 
	RoleManager<Role> roleManager, 
	JwtTokenService jwtTokenService,
	DbContext dbContext,
	IPublishEndpoint publishEndpoint) : IRequestHandler<RegisterUserCommand, Result<string>>
{
	private readonly UserManager<User> _userManager = userManager;
	private readonly RoleManager<Role> _roleManager = roleManager;
	private readonly JwtTokenService _jwtTokenService = jwtTokenService;
	private readonly DbContext _dbContext = dbContext;
	private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

	public async Task<Result<string>> Handle(RegisterUserCommand request, CancellationToken ct)
	{
		foreach (var role in request.Roles ?? Array.Empty<string>())
		{
			if (!await _roleManager.RoleExistsAsync(role))
				return Result<string>.Failure($"Role '{role}' does not exist.");
		}

		var user = new User
		{
			UserName = request.Email,
			Email = request.Email,
			FirstName = request.FirstName,
			LastName = request.LastName
		};
		
		using var transaction = await _dbContext.Database.BeginTransactionAsync(ct);

		try
		{
			var result = await _userManager.CreateAsync(user, request.Password);

			if (!result.Succeeded)
				return Result<string>.Failure(string.Join(", ", result.Errors.Select(e => e.Description)));

			foreach (var role in request.Roles ?? [])
			{
				if (!await _roleManager.RoleExistsAsync(role))
					return Result<string>.Failure($"Issue occured assigning '{role}' role.");
			}

			await _publishEndpoint.Publish(new UserRegistered(user.Id, user.Email), ct);

			await transaction.CommitAsync(ct);
			return Result<string>.Success(_jwtTokenService.GenerateToken(user, request.Roles));
		}
		catch (Exception ex)
		{
			await transaction.RollbackAsync(ct);
			return Result<string>.Failure($"An error occurred while registering the user: {ex.Message}");
		}
	}
}
