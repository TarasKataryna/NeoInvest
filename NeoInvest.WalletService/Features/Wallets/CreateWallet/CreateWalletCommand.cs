using MediatR;

namespace WalletService.Features.Wallets.CreateWallet;

public class CreateWalletCommand : IRequest<Result<Guid>>
{
    public Guid UserId { get; set; }
    public required string Currency { get; set; }
}
