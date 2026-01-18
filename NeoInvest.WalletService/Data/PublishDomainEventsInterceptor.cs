using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NeoInvest.WalletService.Domain.Enitites;

namespace NeoInvest.WalletService.Data;

public class PublishDomainEventsInterceptor(IPublisher publisher) : SaveChangesInterceptor
{
	public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
	{
		if (eventData.Context is not null)
		{
			await PublishEvents(eventData, cancellationToken);
		}

		return await base.SavedChangesAsync(eventData, result, cancellationToken);
	}

	private async Task PublishEvents(SaveChangesCompletedEventData eventData, CancellationToken ct)
	{
		var entities = eventData.Context!.ChangeTracker.Entries<BaseEntity>()
			.Select(e => e.Entity)
			.Where(e => e.DomainEvents.Count != 0)
			.ToList();

		foreach (var entity in entities)
		{
			foreach (var domainEvent in entity.DomainEvents)
			{
				await publisher.Publish(domainEvent, ct);
			}

			entity.ClearDomainEvents();
		}
	}
}
