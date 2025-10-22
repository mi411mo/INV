namespace RTS.Invoicing.Domain.ValueObjects
{
    /// <summary>
    /// Represents a monetary value, combining an amount and a currency.
    /// </summary>
    /// <remarks>
    /// This record is a value object.
    /// </remarks>
    public sealed record Money
    {
        /// <summary>
        /// Gets the monetary amount.
        /// </summary>
        public decimal Amount { get; }

        /// <summary>
        /// Gets the currency unit for the amount.
        /// </summary>
        public Currency Currency { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Money"/> record.
        /// </summary>
        /// <param name="amount">The monetary amount.</param>
        /// <param name="currency">The currency unit.</param>
        public Money(decimal amount, Currency currency)
        {
            Amount = amount;
            Currency = currency;
        }

        /// <summary>
        /// Creates a <see cref="Money"/> instance representing zero value for a specific currency.
        /// </summary>
        /// <param name="currencyCode">The 3-letter ISO 4217 currency code.</param>
        /// <returns>A new <see cref="Money"/> instance with an amount of 0.</returns>
        /// <remarks>
        /// This method will throw an exception if the <paramref name="currencyCode"/> is invalid,
        /// due to the .Value! access on a failed Result.
        /// </remarks>
        public static Money Zero(string currencyCode)
            => new(0, Currency.Create(currencyCode).Value!);
    }
}
