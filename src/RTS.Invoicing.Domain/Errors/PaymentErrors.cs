using RTS.Invoicing.Domain.Common;

namespace RTS.Invoicing.Domain.Errors
{
    public static class PaymentErrors
    {
        public static readonly Error InvalidAmount = new(
            "Payment.InvalidAmount", "Payment amount must be positive.");

        public static readonly Error EmptyReference = new(
            "Payment.EmptyReference", "Transaction reference cannot be empty.");

        public static readonly Error AlreadyProcessed = new(
            "Payment.Status.AlreadyProcessed", "The payment has already been processed and cannot be changed.");
    }
}
