namespace RTS.Invoicing.Domain.Aggregates.Payments.Enums
{
    /// <summary>
    /// Specifies the type of financial transaction recorded.
    /// </summary>
    public enum PaymentType : byte
    {
        /// <summary>
        /// Indicates a standard payment transaction where money is received.
        /// </summary>
        Payment = 1,

        /// <summary>
        /// Indicates a refund transaction where money is returned.
        /// </summary>
        Refund = 2
    }
}
