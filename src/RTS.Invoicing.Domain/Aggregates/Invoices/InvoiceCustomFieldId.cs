namespace RTS.Invoicing.Domain.Aggregates.Invoices
{
    /// <summary>
    /// Represents the unique identifier for a specific custom field entry on an invoice.
    /// </summary>
    /// <param name="Value">The underlying long value of the identifier.</param>
    public sealed record InvoiceCustomFieldId(long Value);
}
