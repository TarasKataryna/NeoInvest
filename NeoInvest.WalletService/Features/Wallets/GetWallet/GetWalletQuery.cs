using MediatR;

namespace NeoInvest.WalletService.Features.Wallets.GetWallet;

public class GetWalletQuery : IRequest<Result<WalletResponse>>
{
    public Guid Id { get; set; }
}