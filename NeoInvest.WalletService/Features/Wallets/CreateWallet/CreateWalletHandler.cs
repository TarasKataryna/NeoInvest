using MediatR;
using Microsoft.EntityFrameworkCore;
using NeoInvest.WalletService;
using NeoInvest.WalletService.Data;
using NeoInvest.WalletService.Domain.Enitites;
using NeoInvest.WalletService.Domain.Enums;

namespace NeoInvest.WalletService.Features.Wallets.CreateWallet;

public class CreateWalletHandler(WalletDbContext dbContext) : IRequestHandler<CreateWalletCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateWalletCommand request, CancellationToken ct)
    {
        var count = await dbContext.Wallets.CountAsync(w => w.UserId == request.UserId, ct);
        if (count >= 5)
        {
            return Result<Guid>.Failure("User cannot have more than 5 wallets.");
        }

        var createResult = Wallet.Create(request.UserId, Enum.Parse<Currency>(request.Currency, true));
        if (!createResult.IsSuccess)
        {
            return Result<Guid>.Failure(createResult.Error!);
		}

		dbContext.Wallets.Add(createResult.Value!);
        await dbContext.SaveChangesAsync(ct);

        return Result<Guid>.Success(createResult.Value!.WalletId);
    }
}
