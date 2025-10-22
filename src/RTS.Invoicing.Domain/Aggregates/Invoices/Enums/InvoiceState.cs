namespace RTS.Invoicing.Domain.Aggregates.Invoices.Enums
{
    /// <summary>
    /// Represents the distinct lifecycle states of an invoice.
    /// </summary>
    public enum InvoiceStatus : byte
    {
        /// <summary>
        /// The invoice is being prepared and can be edited. It has no financial impact yet.
        /// </summary>
        Draft = 1,

        /// <summary>
        /// The invoice has been finalized and sent to the customer. It is now awaiting payment.
        /// This is an Accounts Receivable entry.
        /// </summary>
        Sent = 2,

        /// <summary>
        /// A partial payment has been received, but a balance remains.
        /// </summary>
        PartiallyPaid = 3,

        /// <summary>
        /// The invoice has been paid in full. This is a terminal state.
        /// </summary>
        Paid = 4,

        /// <summary>
        /// The due date has passed, and the invoice has not been fully paid.
        /// This status can trigger reminder workflows or late fees.
        /// </summary>
        Overdue = 5,

        /// <summary>
        /// The invoice was cancelled before being paid. It is non-payable and has no financial value.
        /// </summary>
        Void = 6
    }
}
