using MediatR;
using Microsoft.EntityFrameworkCore;
using NeoInvest.WalletService.Data;
using NeoInvest.WalletService.Domain.Enums;
using NeoInvest.WalletService.Domain.ValueObjects;

namespace NeoInvest.WalletService.Features.Wallets.WithdrawFunds;

public class WithdrawFundsHandler(WalletDbContext dbContext) : IRequestHandler<WithdrawFundsCommand, Result>
{
	public async Task<Result> Handle(WithdrawFundsCommand request, CancellationToken cancellationToken)
	{
		var wallet = await dbContext.Wallets.FirstOrDefaultAsync(w => w.WalletId == request.WalletId, cancellationToken);
		if (wallet is null)
		{
			return Result.Failure("Wallet not found.");
		}
		
		var money = new Money(request.Amount, Enum.Parse<Currency>(request.Currency));

		var withdrawResult = wallet.Withdraw(money);

		if (withdrawResult.IsSuccess)
		{
			await dbContext.SaveChangesAsync(cancellationToken);
		}

		return withdrawResult;
	}
}
