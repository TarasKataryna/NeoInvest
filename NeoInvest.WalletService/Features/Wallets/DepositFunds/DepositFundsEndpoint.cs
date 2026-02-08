using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NeoInvest.WalletService.Features.Wallets.DepositFunds;

public record DepositFundsRequest(string currency, decimal amount);

public static class DepositFundsEndpoint
{
	public static void MapDepositFunds(this IEndpointRouteBuilder builder)
	{
		builder.MapPost("wallets/{id}/withdraw", async (Guid id, [FromBody] DepositFundsRequest request, ISender sender, CancellationToken ct) =>
		{
			var result = await sender.Send(new DepositFundsCommand
			{
				WalletId = id,
				Currency = request.currency,
				Amount = request.amount
			});

			return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
		})
		.RequireAuthorization()
		.WithName("DepositFunds")
		.WithTags("Wallets")
		.Produces(StatusCodes.Status200OK)
		.Produces(StatusCodes.Status400BadRequest);
	}
}
