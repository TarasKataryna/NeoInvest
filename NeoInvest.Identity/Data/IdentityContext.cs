using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NeoInvest.Identity.Entities;

namespace NeoInvest.Identity.Data;

public class IdentityContext : IdentityDbContext<User, Role, Guid>
{
	public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }

	protected IdentityContext()	{ }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.Entity<User>(entity =>
		{
			entity.Property(u => u.Email)
				.IsRequired();

			entity.HasIndex(entity => entity.Email)
				.IsUnique();
		});
	}
}
