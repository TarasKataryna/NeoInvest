using MediatR;
using NeoInvest.WalletService.Domain.Enums;

namespace NeoInvest.WalletService.Domain.Events;

public record WalletDepositEvent : INotification
{
	public Guid WalletId { get; init; }
	public decimal Amount { get; init; }
	public Currency Currency { get; init; }
}
