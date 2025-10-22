using RTS.Invoicing.Domain.Common;
    using System.Text.RegularExpressions;

namespace RTS.Invoicing.Domain.ValueObjects
{
    /// <summary>
    /// Represents a currency, defined by its ISO 4217 code.
    /// </summary>
    /// <remarks>
    /// This class ensures that only valid 3-letter uppercase ISO currency codes can be created.
    /// </remarks>
    public sealed class Currency
    {
        /// <summary>
        /// Compiled regex to validate the ISO 4217 currency code format (three uppercase letters).
        /// </summary>
        private static readonly Regex _isoCodeRegex = new("^[A-Z]{3}$", RegexOptions.Compiled);

        /// <summary>
        /// Gets the 3-letter ISO 4217 currency code.
        /// </summary>
        /// <example>USD</example>
        /// <example>EUR</example>
        public string Code { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Currency"/> class.
        /// </summary>
        /// <param name="code">The 3-letter ISO 4217 currency code.</param>
        private Currency(string code) => Code = code;

        /// <summary>
        /// Creates a new <see cref = "Currency" /> instance from the specified ISO code.
        /// </summary>
        /// <param name="code">The 3-letter ISO 4217 currency code string.</param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing the new <see cref="Currency"/> instance if the code is valid,
        /// or an <see cref="Error"/> if the code is invalid.
        /// </returns>
        public static Result<Currency> Create(string code)
        {
            if (string.IsNullOrWhiteSpace(code) || !_isoCodeRegex.IsMatch(code.ToUpper()))
            {
                return Result.Failure<Currency>(new("Currency.Invalid", "Invalid ISO 4217 currency code."));
            }

            return new Currency(code.ToUpper());
        }
    }
}
