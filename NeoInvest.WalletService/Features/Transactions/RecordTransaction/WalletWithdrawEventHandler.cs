using MediatR;
using NeoInvest.WalletService.Data;
using NeoInvest.WalletService.Domain.Enitites;
using NeoInvest.WalletService.Domain.Events;

namespace NeoInvest.WalletService.Features.Transactions.RecordTransaction;

public class WalletWithdrawEventHandler(WalletDbContext walletDbContext) : INotificationHandler<WalletWithdrawEvent>
{
	public Task Handle(WalletWithdrawEvent notification, CancellationToken cancellationToken)
	{
		walletDbContext.Transactions.Add(Transaction.Create(notification.WalletId, Domain.Enums.TransactionType.Deposit, notification.Amount));

		return walletDbContext.SaveChangesAsync(cancellationToken);
	}
}
