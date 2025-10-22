using RTS.Invoicing.Domain.Common;
using System.Text.RegularExpressions;

namespace RTS.Invoicing.Domain.ValueObjects
{
    public sealed class Currency
    {
        private static readonly Regex _isoCodeRegex = new("^[A-Z]{3}$", RegexOptions.Compiled);
        public string Code { get; }

        /// <summary>
/// Initializes a new instance of <see cref="Currency"/> with the provided currency code.
/// </summary>
/// <param name="code">The ISO 4217 three-letter currency code, expected to be validated and normalized to upper-case.</param>
private Currency(string code) => Code = code;

        /// <summary>
        /// Creates a Currency value object from an ISO 4217 three-letter currency code.
        /// </summary>
        /// <param name="code">The ISO 4217 currency code (three ASCII letters); case is normalized to upper-case.</param>
        /// <returns>
        /// A <see cref="Result{Currency}"/> containing the created <see cref="Currency"/> when <paramref name="code"/> is a valid ISO 4217 three-letter code; otherwise a failure Result with error code "Currency.Invalid" and message "Invalid ISO 4217 currency code.".
        /// </returns>
        public static Result<Currency> Create(string code)
        {
            if (string.IsNullOrWhiteSpace(code) || !_isoCodeRegex.IsMatch(code))
            {
                return Result.Failure<Currency>(new("Currency.Invalid", "Invalid ISO 4217 currency code."));
            }

            return new Currency(code.ToUpper());
        }
    }
}