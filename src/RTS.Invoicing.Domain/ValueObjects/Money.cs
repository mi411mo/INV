namespace RTS.Invoicing.Domain.ValueObjects
{
    public sealed record Money
    {
        public decimal Amount { get; }
        public Currency Currency { get; }

        public Money(decimal amount, Currency currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public static Money Zero(string currencyCode) => new(0, Currency.Create(currencyCode).Value!);
    }
}
