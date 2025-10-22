using RTS.Invoicing.Domain.Common;

namespace RTS.Invoicing.Domain.ValueObjects
{
    public sealed record InvoiceNumber
    {
        public string Value { get; }

        private InvoiceNumber(string value) => Value = value;

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
