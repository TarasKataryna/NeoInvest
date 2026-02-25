using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace NeoInvest.Saga;

public class SagaDbContext : DbContext
{
	public SagaDbContext(DbContextOptions<SagaDbContext> options) : base(options) { }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.AddInboxStateEntity();
		modelBuilder.AddOutboxMessageEntity();
		modelBuilder.AddOutboxStateEntity();

		modelBuilder.Entity<OnboardingSagaState>().HasKey(s => s.CorrelationId);
	}
}
