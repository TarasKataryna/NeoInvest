using MediatR;

namespace NeoInvest.WalletService.Features.Wallets.WithdrawFunds;

public class WithdrawFundsCommand : IRequest<Result>
{
    public Guid WalletId { get; set; }
    public required string Currency { get; set; }
    public decimal Amount { get; set; }
}