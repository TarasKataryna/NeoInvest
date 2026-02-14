using MassTransit;
using MediatR;
using NeoInvest.Shared.Events;
using NeoInvest.WalletService.Features.Wallets.CreateWallet;

namespace NeoInvest.WalletService.EventHandler;

public class UserRegisteredHandler(ISender sender, ILogger<UserRegisteredHandler> logger) : IConsumer<UserRegistered>
{
	private readonly ISender _sender = sender;
	private readonly ILogger<UserRegisteredHandler> _logger = logger;

	public Task Consume(ConsumeContext<UserRegistered> context)
	{
		_logger.LogInformation("User registered with email: {Email}", context.Message.Email);

		return _sender.Send(new CreateWalletCommand
		{
			UserId = context.Message.Id,
			Currency = "USD"
		});
	}
}
