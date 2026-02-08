using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NeoInvest.WalletService.Features.Wallets.WithdrawFunds;

public record WithdrawFundsRequest(string currency, decimal amount);

public static class WithdrawFundsEndpoint
{
	public static void MapWithdrawFunds(this IEndpointRouteBuilder builder)
	{
		builder.MapPost("wallets/{id}/withdraw", async (Guid id, [FromBody] WithdrawFundsRequest request, ISender sender, CancellationToken ct) =>
		{
			var result = await sender.Send(new WithdrawFundsCommand
			{
				WalletId = id,
				Currency = request.currency,
				Amount = request.amount
			}, ct);

			return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
		})
		.RequireAuthorization()
		.WithName("WithdrawFunds")
		.WithTags("Wallets")
		.Produces(StatusCodes.Status200OK)
		.Produces(StatusCodes.Status400BadRequest);
	}
}
