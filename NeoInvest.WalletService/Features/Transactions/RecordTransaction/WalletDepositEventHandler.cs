using MediatR;
using NeoInvest.WalletService.Data;
using NeoInvest.WalletService.Domain.Enitites;
using NeoInvest.WalletService.Domain.Enums;
using NeoInvest.WalletService.Domain.Events;

namespace NeoInvest.WalletService.Features.Transactions.RecordTransaction;

public class WalletDepositEventHandler(WalletDbContext walletDbContext) : INotificationHandler<WalletDepositEvent>
{
	public Task Handle(WalletDepositEvent notification, CancellationToken cancellationToken)
	{
		walletDbContext.Transactions.Add(Transaction.Create(notification.WalletId, TransactionType.Deposit, notification.Amount));

		return walletDbContext.SaveChangesAsync(cancellationToken);
	}
}
