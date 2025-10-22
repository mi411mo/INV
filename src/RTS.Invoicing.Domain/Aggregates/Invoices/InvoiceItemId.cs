namespace RTS.Invoicing.Domain.Aggregates.Invoices
{
    /// <summary>
    /// Represents the unique identifier for an invoice line item.
    /// </summary>
    /// <param name="Value">The underlying <see cref="long" /> value of the identifier.</param>
    public record InvoiceItemId(long Value);
}
