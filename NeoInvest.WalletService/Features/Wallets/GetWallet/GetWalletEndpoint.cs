using MediatR;

namespace NeoInvest.WalletService.Features.Wallets.GetWallet;

public static class GetWalletEndpoint
{
	public static void MapGetWallet(this IEndpointRouteBuilder app)
	{
		app.MapPost("/wallets/{id}", async (Guid id, ISender sender) =>
		{
			return await sender.Send(new GetWalletQuery
			{
				Id = id
			}) switch
			{
				{ IsSuccess: true } success => Results.Ok(success.Value),
				{ } errors => Results.BadRequest(errors)
			};
		})
		.RequireAuthorization()
		.WithTags("Wallets")
		.WithName("GetWallet");
	}
}
