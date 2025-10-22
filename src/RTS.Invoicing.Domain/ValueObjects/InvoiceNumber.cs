using RTS.Invoicing.Domain.Common;

namespace RTS.Invoicing.Domain.ValueObjects
{
    public sealed record InvoiceNumber
    {
        public string Value { get; }

        /// <summary>
/// Initializes a new instance of <see cref="InvoiceNumber"/> with the specified invoice value.
/// </summary>
/// <param name="value">The invoice number string to store as the value.</param>
private InvoiceNumber(string value) => Value = value;

        /// <summary>
        /// Creates an <see cref="InvoiceNumber"/> value object from the provided string.
        /// </summary>
        /// <param name="value">The invoice number string to validate and wrap.</param>
        /// <returns>
        /// A successful <see cref="Result{InvoiceNumber}"/> containing the new <see cref="InvoiceNumber"/> when <paramref name="value"/> is not null, empty, or whitespace; otherwise a failed <see cref="Result{InvoiceNumber}"/> with code "InvoiceNumber.Empty" and message "Invoice number cannot be empty.".
        /// </returns>
        public static Result<InvoiceNumber> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<InvoiceNumber>(new("InvoiceNumber.Empty", "Invoice number cannot be empty."));
            }
            return new InvoiceNumber(value);
        }
    }
}