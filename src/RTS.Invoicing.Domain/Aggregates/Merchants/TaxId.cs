namespace RTS.Invoicing.Domain.Aggregates.Merchants
{
    /// <summary>
    /// Represents the unique identifier for a tax definition.
    /// </summary>
    /// <param name="Value">The underlying <see cref="long" /> value of the identifier.</param>
    public sealed record TaxId(long Value);
}
