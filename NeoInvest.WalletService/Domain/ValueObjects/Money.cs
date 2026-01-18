using WalletService.Domain.Enums;

namespace NeoInvest.WalletService.Domain.ValueObjects;

public record Money
{
    public decimal Amount { get; init; }
    public Currency Currency { get; init; }

    public static Money Zero(Currency currency) => new(0m, currency);

    public static Money operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException("Cannot add Money with different currencies.");
        return new Money(a.Amount + b.Amount, a.Currency);
    }

    public static Money operator -(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException("Cannot subtract Money with different currencies.");
        return new Money(a.Amount - b.Amount, a.Currency);
    }

    public Money(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }
}
