using MediatR;

namespace NeoInvest.WalletService.Features.Wallets.DepositFunds;

public class DepositFundsCommand : IRequest<Result>
{
    public Guid WalletId { get; set; }
    public required string Currency { get; set; }
    public decimal Amount { get; set; }
}