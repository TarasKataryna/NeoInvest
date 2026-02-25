using MassTransit;
using NeoInvest.Shared.Commands;
using NeoInvest.Shared.Events;

namespace NeoInvest.Saga;

//testing saga
public class OnboardingSaga : MassTransitStateMachine<OnboardingSagaState>
{
	public State CreatingWallet { get; private set; }
	
	public Event<UserRegistered> UserRegistered { get; private set; }
	public Event<WalletCreated> WalletCreated { get; private set; }

	public OnboardingSaga()
	{
		InstanceState(x => x.CurrentState);

		Event(() => UserRegistered, x => x.CorrelateById(context => context.Message.Id));
		Event(() => WalletCreated, x => x.CorrelateById(context => context.Message.UserId));

		Initially(
			When(UserRegistered)
				.Then(context =>
				{
					context.Saga.UserId = context.Message.Id;
					context.Saga.Email = context.Message.Email;
					context.Saga.CreateDate = DateTime.UtcNow;
				})
				.TransitionTo(CreatingWallet)
				.Publish(context => new CreateUserWallet(context.Message.Id, "USD"))
		);

		During(CreatingWallet,
			When(WalletCreated)
				.Then(context => Console.WriteLine($"Sage is finished for user: {context.Message.UserId}"))
				.Finalize());

		SetCompletedWhenFinalized();
	}
}
