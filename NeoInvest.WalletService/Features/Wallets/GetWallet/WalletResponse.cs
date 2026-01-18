using NeoInvest.WalletService.Domain.Enums;

namespace NeoInvest.WalletService.Features.Wallets.GetWallet;

public class WalletResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Currency Currency { get; set; }
}
