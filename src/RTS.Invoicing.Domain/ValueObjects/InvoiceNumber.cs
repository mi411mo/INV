using RTS.Invoicing.Domain.Common;

namespace RTS.Invoicing.Domain.ValueObjects
{
    /// <summary>
    /// Represents a unique invoice number as a value object.
    /// </summary>
    /// <remarks>
    /// This record ensures that an invoice number cannot be null or empty.
    /// </remarks>
    public sealed record InvoiceNumber
    {
        /// <summary>
        /// Gets the string value of the invoice number.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvoiceNumber"/> record.
        /// </summary>
        /// <param name="value">The non-empty string value of the invoice number.</param>
        private InvoiceNumber(string value) => Value = value;

        /// <summary>
        /// Creates a new <see cref="InvoiceNumber"/> instance.
        /// </summary>
        /// <param name="value">The string value for the invoice number.</param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing the new <see cref="InvoiceNumber"/> instance if the value is valid,
        /// or an <see cref="Error"/> if the value is null or empty.
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
