using MassTransit;
using MediatR;
using NeoInvest.Shared.Commands;
using NeoInvest.WalletService.Features.Wallets.CreateWallet;

namespace NeoInvest.WalletService.EventHandler;

public class CreateUserWalletConsumer(ISender sender, ILogger<CreateUserWalletConsumer> logger) : IConsumer<CreateUserWallet>
{
	private readonly ISender _sender = sender;
	private readonly ILogger<CreateUserWalletConsumer> _logger = logger;

	public Task Consume(ConsumeContext<CreateUserWallet> context)
	{
		_logger.LogInformation("Creating wallet for user: {UserId}", context.Message.UserId);

		return _sender.Send(new CreateWalletCommand
		{
			UserId = context.Message.UserId,
			Currency = context.Message.Currency
		});
	}
}
