using FluentValidation;

namespace WalletService.Features.Wallets.CreateWallet;

public class CreateWalletValidator : AbstractValidator<CreateWalletCommand>
{
    public CreateWalletValidator()
    {
        RuleFor(c => c.UserId)
            .NotEmpty()
            .NotNull()
            .WithMessage("UserId is required.");
    }
}
