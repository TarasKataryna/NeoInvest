using MediatR;

namespace NeoInvest.WalletService.Features.Wallets.CreateWallet;

public record CreateWallerRequest(Guid UserId, string Currency);

public static class CreateWalletEndpoint
{
	public static void MapCreateWallet(this IEndpointRouteBuilder app)
	{
		app.MapPost("/wallets", async (CreateWallerRequest request, ISender sender) =>
		{
			return await sender.Send(new CreateWalletCommand
			{
				UserId = request.UserId,
				Currency = request.Currency
			}) switch
			{
				{ IsSuccess: true } success => Results.Ok(success.Value),
				{ } errors => Results.BadRequest(errors)
			};
		})
		.RequireAuthorization()
		.WithTags("Wallets")
		.WithName("CreateWallet");
	}
}
