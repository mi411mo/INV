namespace RTS.Invoicing.Domain.Entities.Payments.Enums
{
    /// <summary>
    /// Represents the status of a single payment.
    /// </summary>
    public enum PaymentStatus : byte
    {
        /// <summary>
        /// The payment process has been initiated but is not yet confirmed.
        /// This is the initial, non-terminal state.
        /// </summary>
        Pending = 1,

        /// <summary>
        /// The payment was successfully processed.
        /// </summary>
        Succeeded = 2,

        /// <summary>
        /// The payment could not be processed (e.g., declined card, insufficient funds).
        /// </summary>
        Failed = 3,

        /// <summary>
        /// The payment was explicitly cancelled by a user or the system before completion. 
        /// This is a terminal state.
        /// </summary>
        Cancelled = 4,

        /// <summary>
        /// The payment was not completed within the allowed time frame. This is a terminal state.
        /// </summary>
        Expired = 5,
    }
}
