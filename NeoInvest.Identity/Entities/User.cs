using Microsoft.AspNetCore.Identity;

namespace NeoInvest.Identity.Entities;

public class User : IdentityUser<Guid>
{
	public required string FirstName { get; set; }
	public required string LastName { get; set; }
	public override required string? Email { get; set; }

	public DateTimeOffset CreateDate { get; set; }
}
