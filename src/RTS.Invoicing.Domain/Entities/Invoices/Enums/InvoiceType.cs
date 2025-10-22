namespace RTS.Invoicing.Domain.Entities.Invoices.Enums
{
    /// <summary>
    /// Defines the legal and financial nature of an invoice document.
    /// </summary>
    public enum InvoiceType : byte
    {
        /// <summary>
        /// A standard commercial document issued by a merchant (seller) to a customer (buyer), indicating the products, quantities, and agreed prices for products or services.
        /// This document increases the amount owed by the customer.
        /// </summary>
        StandardInvoice = 1,

        /// <summary>
        /// A commercial document issued by a merchant (seller) to a customer (buyer) to reduce the amount owed from an earlier invoice.
        /// It is used for returns, corrections, or allowances. This document has a negative value.
        /// </summary>
        CreditNote = 2,

        /// <summary>
        /// A commercial document issued by a seller to a customer (buyer) to increase the amount owed from an earlier invoice.
        /// Used to correct an under-billing. This document has a positive value.
        /// </summary>
        DebitNote = 3
    }
}
