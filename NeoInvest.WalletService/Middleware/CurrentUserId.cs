namespace NeoInvest.WalletService.Middleware;

public record CurrentUser(Guid Id)
{
	public static ValueTask<CurrentUser?> BindAsync(HttpContext context)
	{
		var claim = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

		if (claim is null || !Guid.TryParse(claim.Value, out var userId))
		{
			return ValueTask.FromResult<CurrentUser?>(null);
		}

		return ValueTask.FromResult<CurrentUser?>(new CurrentUser(userId));
	}

	public static implicit operator Guid(CurrentUser currentUser) => currentUser.Id;
}
