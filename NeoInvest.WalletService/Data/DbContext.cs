using MediatR;
using Microsoft.EntityFrameworkCore;
using NeoInvest.WalletService.Domain.Enitites;

namespace NeoInvest.WalletService.Data;

public class WalletDbContext(DbContextOptions<WalletDbContext> options, IPublisher publisher) : DbContext(options)
{
	private readonly IPublisher _publisher = publisher;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(WalletDbContext).Assembly);
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		var result = base.SaveChangesAsync(cancellationToken);
		
		var entities = ChangeTracker.Entries<BaseEntity>()
			.Select(e => e.Entity)
			.Where(e => e.DomainEvents.Count != 0)
			.ToList();

		foreach (var entity in entities)
		{
			foreach (var domainEvent in entity.DomainEvents)
			{
				_publisher.Publish(domainEvent, cancellationToken);
			}

			entity.ClearDomainEvents();
		}
		
		return result;
	}

	public DbSet<Wallet> Wallets { get; set; }
	public DbSet<Transaction> Transactions { get; set; }
}
