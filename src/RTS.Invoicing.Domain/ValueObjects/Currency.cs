using RTS.Invoicing.Domain.Common;
using System.Text.RegularExpressions;

namespace RTS.Invoicing.Domain.ValueObjects
{
    public sealed class Currency
    {
        private static readonly Regex _isoCodeRegex = new("^[A-Z]{3}$", RegexOptions.Compiled);
        public string Code { get; }

        private Currency(string code) => Code = code;

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
