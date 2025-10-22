using RTS.Invoicing.Domain.Common;

namespace RTS.Invoicing.Domain.Errors
{
    /// <summary>
    /// Contains predefined <see cref="Error"/> constants related to invoice number sequencing.
    /// </summary>
    public static class InvoiceSequenceErrors
    {
        /// <summary>
        /// Represents an error indicating that the prefix for an invoice number sequence is required and cannot be empty.
        /// </summary>
        public static readonly Error PrefixRequired = new(
            "InvoiceSequence.Prefix.Required", "The invoice number prefix cannot be empty.");

        /// <summary>
        /// Represents an error indicating that the invoice number prefix
        /// exceeds the maximum allowed character limit.
        /// </summary>
        public static readonly Error PrefixTooLong = new(
            "InvoiceSequence.Prefix.TooLong", $"The invoice number prefix cannot exceed '{Constants.MAX_PREFIX_CHARACTERS}' characters.");
    }
}
