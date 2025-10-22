using RTS.Invoicing.Domain.Common;

namespace RTS.Invoicing.Domain.Errors
{
    public static class InvoiceSequenceErrors
    {
        public static readonly Error PrefixRequired = new(
            "InvoiceSequence.Prefix.Required", "The invoice number prefix cannot be empty.");

        public static readonly Error PrefixTooLong = new(
            "InvoiceSequence.Prefix.TooLong", $"The invoice number prefix cannot exceed '{Constants.MAX_PREFIX_CHARACTERS}' characters.");
    }
}
