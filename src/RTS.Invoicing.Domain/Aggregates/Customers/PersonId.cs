namespace RTS.Invoicing.Domain.Aggregates.Customers
{
    /// <summary>
    /// Represents the unique identifier for a person.
    /// </summary>
    /// <param name="Value">The underlying <see cref="long" /> value of the identifier.</param>
    public sealed record PersonId(long Value);
}
