using RTS.Invoicing.Domain.Common;

namespace RTS.Invoicing.Domain.Errors
{
    /// <summary>
    /// Contains predefined <see cref="Error"/> constants related to payment operations.
    /// </summary>
    public static class PaymentErrors
    {
        /// <summary>
        /// Represents an error indicating that the payment amount must be a positive value.
        /// </summary>
        public static readonly Error InvalidAmount = new(
            "Payment.InvalidAmount", "Payment amount must be positive.");

        /// <summary>
        /// Represents an error indicating that the payment's transaction reference cannot be empty.
        /// </summary>
        public static readonly Error EmptyReference = new(
            "Payment.EmptyReference", "Transaction reference cannot be empty.");

        /// <summary>
        /// Represents an error indicating that an operation cannot be performed
        /// because the payment has already been finalized or processed.
        /// </summary>
        public static readonly Error AlreadyProcessed = new(
            "Payment.Status.AlreadyProcessed", "The payment has already been processed and cannot be changed.");
    }
}
