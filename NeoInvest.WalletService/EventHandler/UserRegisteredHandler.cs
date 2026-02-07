using MassTransit;
using NeoInvest.Shared.Events;

namespace NeoInvest.WalletService.EventHandler;

public class UserRegisteredHandler(ILogger<UserRegisteredHandler> logger) : IConsumer<UserRegistered>
{
	private readonly ILogger<UserRegisteredHandler> _logger = logger;

	public Task Consume(ConsumeContext<UserRegistered> context)
	{
		_logger.LogInformation("User registered with email: {Email}", context.Message.Email);

		return Task.CompletedTask;
	}
}
