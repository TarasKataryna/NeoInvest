using NeoInvest.WalletService.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeoInvest.WalletService.Domain.Enitites;

public class Transaction
{
	[Key]
	public Guid TansactionId { get; set; }
	public Guid WalletId { get; set; }
	public TransactionType TransactionType { get; set; }
	public decimal Amount { get; set; }
	public DateTimeOffset CreateDate { get; set; }

	[ForeignKey(nameof(WalletId))]
	public virtual Wallet? Wallet { get; set; }

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
