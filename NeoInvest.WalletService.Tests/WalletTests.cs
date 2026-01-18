using FluentAssertions;
using NeoInvest.WalletService.Domain.Enitites;
using NeoInvest.WalletService.Domain.Enums;
using NeoInvest.WalletService.Domain.Events;
using NeoInvest.WalletService.Domain.ValueObjects;

namespace NeoInvest.WalletService.Tests;

public class WalletTests
{
	[Fact]
	public void Wallet_Cannot_Accept_Empty_UserId()
	{
		var userId = Guid.Empty;
		var currency = Currency.Usd;

		var createResult = Wallet.Create(userId, currency);
		
		createResult.IsFailure.Should().BeTrue();
		createResult.Error.Should().Be("UserId cannot be empty.");
	}

	[Fact]
	public void Deposit_Should_ReturnFailure_When_Different_Currencies()
	{
		var userId = Guid.NewGuid();
		var currency = Currency.Eur;
		var wallet = Wallet.Create(userId, currency).Value!;

		var result = wallet.Deposit(new Money(50, Currency.Usd));

		result.IsFailure.Should().BeTrue();
		result.Error.Should().Be("Currency mismatch.");
		wallet.Balance.Should().Be(0);
	}

	[Fact]
	public void Deposit_Should_Raise_WalletDepositEvent_When_Successful()
	{
		var wallet = Wallet.Create(Guid.NewGuid(), Currency.Usd).Value!;
		wallet.Deposit(new Money(100, Currency.Usd));

		wallet.DomainEvents.Should().ContainSingle(e => e is WalletDepositEvent);

		var @event = wallet.DomainEvents.OfType<WalletDepositEvent>().Single();
		@event.Amount.Should().Be(100);
		@event.WalletId.Should().Be(wallet.WalletId);
	}

	[Fact]
	public void Withdraw_Should_ReturnFailure_When_Different_Currencies()
	{
		var userId = Guid.NewGuid();
		var currency = Currency.Eur;
		var wallet = Wallet.Create(userId, currency).Value!;

		wallet.Deposit(new Money(50, currency));

		var amountToWithdraw = new Money(100, Currency.Usd);

		var result = wallet.Withdraw(amountToWithdraw);

		result.IsFailure.Should().BeTrue();
		result.Error.Should().Be("Currency mismatch.");
		wallet.Balance.Should().Be(50);
	}

	[Fact]
	public void Withdraw_Should_ReturnFailure_When_BalanceIsInsufficient()
	{
		var userId = Guid.NewGuid();
		var currency = Currency.Usd;
		var wallet = Wallet.Create(userId, currency).Value!;
		
		wallet.Deposit(new Money(50, Currency.Usd));

		var amountToWithdraw = new Money(100, currency);

		var result = wallet.Withdraw(amountToWithdraw);

		result.IsFailure.Should().BeTrue();
		result.Error.Should().Be("Insufficient funds.");
		wallet.Balance.Should().Be(50);
	}

	[Fact]
	public void Withdraw_Should_Raise_WalletWithdrawEvent_When_Successful()
	{
		var wallet = Wallet.Create(Guid.NewGuid(), Currency.Usd).Value!;
		wallet.Deposit(new Money(100, Currency.Usd));
		var amount = new Money(40, Currency.Usd);

		wallet.Withdraw(amount);

		wallet.DomainEvents.Should().ContainSingle(e => e is WalletWithdrawEvent);

		var @event = wallet.DomainEvents.OfType<WalletWithdrawEvent>().Single();
		@event.Amount.Should().Be(40);
		@event.WalletId.Should().Be(wallet.WalletId);
	}

	[Fact]
	public void Adding_Different_Currencies_Should_Throw_InvalidOperationException()
	{
		var usd = new Money(10, Currency.Usd);
		var eur = new Money(10, Currency.Eur);

		Action action = () => { var result = usd + eur; };

		action.Should().Throw<InvalidOperationException>()
			  .WithMessage("Cannot add Money with different currencies.");
	}
}
