using MediatR;

namespace NeoInvest.WalletService.Domain.Enitites;

public class BaseEntity
{
	private readonly List<INotification> _domainEvents = [];
	public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

	protected void AddDomainEvent(INotification eventItem) => _domainEvents.Add(eventItem);

	public void ClearDomainEvents() => _domainEvents.Clear();
}
