using NeoInvest.WalletService.Domain.Enums;

namespace NeoInvest.WalletService.Domain.Enitites;

public class Transaction
{
	public Guid TansactionId { get; set; }
	public Guid WalletId { get; set; }
	public TransactionType TransactionType { get; set; }
	public decimal Amount { get; set; }
	public DateTimeOffset CreateDate { get; set; }

	private Transaction() { }

	public static Transaction Create(Guid walletId, TransactionType transactionType, decimal amount)
	{
		return new Transaction
		{
			TansactionId = Guid.NewGuid(),
			WalletId = walletId,
			TransactionType = transactionType,
			Amount = amount,
			CreateDate = DateTimeOffset.UtcNow
		};
	}
}
