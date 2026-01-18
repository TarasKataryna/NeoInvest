using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NeoInvest.WalletService.Domain.Enitites;
using Npgsql;

namespace NeoInvest.WalletService.Features.Wallets.GetWallet;

public class GetWalletHandler(NpgsqlDataSource dataSource) : IRequestHandler<GetWalletQuery, Result<WalletResponse>>
{
    public async Task<Result<WalletResponse>> Handle(GetWalletQuery request, CancellationToken ct)
    {
        var sql = @$"select id as {nameof(Wallet.WalletId)}
                        user_id as {nameof(Wallet.UserId)}
                        currency as {nameof(Wallet.Currency)}
                    from Wallet where id = @Id";

        using var connection = await dataSource.OpenConnectionAsync(ct);

        return await connection.QuerySingleOrDefaultAsync<Wallet>(sql, new { request.Id }) switch
        {
            null => Result<WalletResponse>.Failure("Wallet not found"),
            var wallet => Result<WalletResponse>.Success(new WalletResponse
            {
                Id = wallet.WalletId,
                UserId = wallet.UserId,
                Currency = wallet.Currency
            })
        };
    }
}
