using MediatR;

namespace WalletService.Features.Wallets.GetWallet;

public class GetWalletQuery : IRequest<Result<WalletResponse>>
{
    public Guid Id { get; set; }
}