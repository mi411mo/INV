namespace RTS.Invoicing.Domain.ValueObjects
{
    public sealed record Money
    {
        public decimal Amount { get; }
        public Currency Currency { get; }

        /// <summary>
        /// Initializes a new Money instance with the specified monetary amount and currency.
        /// </summary>
        /// <param name="amount">The monetary amount.</param>
        /// <param name="currency">The currency associated with the amount.</param>
        public Money(decimal amount, Currency currency)
        {
            Amount = amount;
            Currency = currency;
        }

        /// <summary>
/// Creates a Money instance representing zero amount in the specified currency.
/// </summary>
/// <param name="currencyCode">The currency code (e.g., "USD") used to determine the currency.</param>
/// <returns>A Money instance with Amount equal to 0 and the currency identified by <paramref name="currencyCode"/>.</returns>
public static Money Zero(string currencyCode) => new(0, Currency.Create(currencyCode).Value!);
    }
}