using NeoInvest.WalletService.Domain.ValueObjects;
using WalletService.Domain.Enums;

namespace WalletService.Domain.Enitites;

public class Wallet
{
	public Guid Id { get; private set; }
	public Guid UserId { get; private set; }
	public Currency Currency { get; private set; }
	public decimal Balance { get; private set; }
	public DateTimeOffset CreatedDate { get; private set; }

	private int WithdrawLimit => 100000;

	private Wallet() { }

	public static Result<Wallet> Create(Guid userId, Currency currency)
	{
		if (userId == Guid.Empty)
			return Result<Wallet>.Failure("UserId cannot be empty.");

		var wallet = new Wallet
		{
			Id = Guid.NewGuid(),
			UserId = userId,
			Currency = currency,
			Balance = 0,
			CreatedDate = DateTimeOffset.UtcNow
		};

		return Result<Wallet>.Success(wallet);
	}

	public Result Withdraw(Money money)
	{
		if (money.Currency != Currency)
			return Result.Failure("Currency mismatch.");

		if (money.Amount <= 0)
			return Result.Failure("Withdrawal amount must be positive.");

		if (Balance < money.Amount)
			return Result.Failure("Insufficient funds.");

		if (money.Amount > WithdrawLimit)
			return Result.Failure($"Withdrawal amount exceeds the limit. Limit is - {WithdrawLimit}");

		Balance -= money.Amount;
		return Result.Success();
	}

	public Result Deposit(Money money)
	{
		if (money.Currency != Currency)
			return Result.Failure("Currency mismatch.");

		if (money.Amount <= 0)
			return Result.Failure("Deposit amount must be positive.");

		Balance += money.Amount;
		return Result.Success();
	}
}
