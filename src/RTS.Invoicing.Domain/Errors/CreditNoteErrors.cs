using RTS.Invoicing.Domain.Common;

namespace RTS.Invoicing.Domain.Errors
{
    public static class CreditNoteErrors
    {
        /// <summary>
        /// The invalid original.
        /// </summary>
        public static readonly Error InvalidOriginal = new(
            "CreditNote.InvalidOriginal", "Cannot create a credit note for a draft invoice.");
    }
}
