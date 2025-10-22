namespace RTS.Invoicing.Domain.Aggregates.Merchants
{
    /// <summary>
    /// Represents the unique identifier for an invoice number sequence.
    /// </summary>
    /// <param name="Value">The underlying <see cref="long" /> value of the identifier.</param>
    public sealed record InvoiceSequenceId(long Value);
}
