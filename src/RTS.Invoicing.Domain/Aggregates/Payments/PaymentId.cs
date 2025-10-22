namespace RTS.Invoicing.Domain.Aggregates.Payments
{
    /// <summary>
    /// Represents the unique identifier for a payment.
    /// </summary>
    /// <param name="Value">The underlying <see cref="long" /> value of the identifier.</param>
    public sealed record PaymentId(long Value);
}
