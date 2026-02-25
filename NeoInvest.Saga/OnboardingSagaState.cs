using MassTransit;

namespace NeoInvest.Saga;

public class OnboardingSagaState : SagaStateMachineInstance
{
	public Guid CorrelationId { get; set; }
	public string? CurrentState { get; set; }

	public required Guid UserId { get; set; }
	public required string Email { get; set; }
	public required DateTime CreateDate { get; set; }
}
