using MediatR;
using Microsoft.EntityFrameworkCore;
using NeoInvest.WalletService.Domain.ValueObjects;
using WalletService;
using WalletService.Data;
using WalletService.Domain.Enums;

namespace NeoInvest.WalletService.Features.Wallets.DepositFunds;

public class DepositFundsHandler(WalletDbContext dbContext) : IRequestHandler<DepositFundsCommand, Result>
{
	public async Task<Result> Handle(DepositFundsCommand request, CancellationToken cancellationToken)
	{
		var wallet = await dbContext.Wallets.FirstOrDefaultAsync(w => w.Id == request.WalletId, cancellationToken);
		if (wallet is null)
		{
			return Result.Failure("Wallet not found.");
		}
		
		var money = new Money(request.Amount, Enum.Parse<Currency>(request.Currency));

		var withdrawResult = wallet.Deposit(money);

		if (withdrawResult.IsSuccess)
		{
			await dbContext.SaveChangesAsync(cancellationToken);
		}

		return withdrawResult;
	}
}
