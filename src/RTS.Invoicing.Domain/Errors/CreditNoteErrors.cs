using RTS.Invoicing.Domain.Common;

namespace RTS.Invoicing.Domain.Errors
{
    /// <summary>
    /// Contains predefined <see cref="Error"/> constants related to credit note operations.
    /// </summary>
    public static class CreditNoteErrors
    {
        /// <summary>
        /// Represents an error indicating that a credit note cannot be created for an invoice
        /// that is still in a draft state.
        /// </summary>
        public static readonly Error InvalidOriginal = new(
            "CreditNote.InvalidOriginal", "Cannot create a credit note for a draft invoice.");
    }
}
