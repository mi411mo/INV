using RTS.Invoicing.Domain.Common;

namespace RTS.Invoicing.Domain.Errors
{
    /// <summary>
    /// Contains predefined <see cref="Error"/> constants related to invoice operations.
    /// </summary>
    public static class InvoiceErrors
    {
        /// <summary>
        /// Represents an error indicating that an action requires the invoice
        /// to be in a draft state, but it is not.
        /// </summary>
        public static readonly Error NotDraft = new(
            "Invoice.NotDraft", "The action can only be performed on a draft invoice.");

        /// <summary>
        /// Represents an error indicating that an invoice cannot be sent
        /// because it does not contain any line items.
        /// </summary>
        public static readonly Error NoItems = new(
            "Invoice.NoItems", "Cannot send an invoice with no items.");

        /// <summary>
        /// Represents an error indicating that a cancellation operation failed
        /// because the invoice has already received payments.
        /// </summary>
        public static readonly Error IsPaid = new(
            "Invoice.IsPaid", "Cannot cancel an invoice that has received payments. Consider a credit note instead.");

        /// <summary>
        /// Represents an error indicating that a payment cannot be applied
        /// because the invoice is already paid or has been cancelled.
        /// </summary>
        public static readonly Error InvalidStatusForPayment = new(
            "Invoice.InvalidStatusForPayment", "Payment cannot be applied to a paid or cancelled invoice.");

        /// <summary>
        /// Represents an error indicating that the "due days" value is negative, which is invalid.
        /// </summary>
        public static readonly Error InvalidDueDays = new(
           "Invoice.InvalidDueDays", "Due days cannot be negative.");

        /// <summary>
        /// Represents an error indicating that the invoice number is null or empty.
        /// </summary>
        public static readonly Error InvalidNumber = new(
            "Invoice.InvalidNumber", "Invoice number cannot be empty.");

        /// <summary>
        /// Represents an error indicating that the invoice's issue date is set to a future date.
        /// </summary>
        public static readonly Error IssueDateInFuture = new(
            "Invoice.IssueDateInFuture", "Issue date cannot be in the future.");

        /// <summary>
        /// Represents an error indicating that the invoice's due date is earlier than its issue date.
        /// </summary>
        public static readonly Error DueDateBeforeIssueDate = new(
            "Invoice.DueDateBeforeIssueDate", "Due date cannot be before the issue date.");

        /// <summary>
        /// Represents an error indicating that an invoice cannot be sent
        /// because it has no line items.
        /// </summary>
        public static readonly Error CannotSendEmptyInvoice = new(
            "Invoice.CannotSendEmpty", "Cannot send an invoice with no items.");

        /// <summary>
        /// Represents an error indicating that a specified invoice item
        /// could not be found within the invoice.
        /// </summary>
        public static readonly Error ItemNotFound = new(
            "Invoice.ItemNotFound", "The specified invoice item was not found.");

        /// <summary>
        /// Represents an error indicating that the invoice is not in a valid state
        /// (e.g., 'Sent' or 'Overdue') to be marked as fully paid.
        /// </summary>
        public static readonly Error InvalidStateForPayment = new(
            "Invoice.InvalidStateForPayment", "Invoice is not in a state that can be marked as paid.");

        /// <summary>
        /// Represents an error indicating that the invoice is not in a valid state
        /// (e.g., 'Sent' or 'Overdue') to receive a partial payment.
        /// </summary>
        public static readonly Error InvalidStateForPartialPayment = new(
            "Invoice.InvalidStateForPartialPayment", "Invoice is not in a state that can be partially paid.");

        /// <summary>
        /// Represents an error indicating that a void operation cannot be performed
        /// because the invoice is paid or partially paid.
        /// </summary>
        public static readonly Error CannotVoidPaidInvoice = new(
            "Invoice.CannotVoidPaid", "A paid or partially paid invoice cannot be voided. A credit note should be issued instead.");

        /// <summary>
        /// Represents an error indicating that a credit note cannot be created
        /// for an invoice that has not been sent to the customer yet.
        /// </summary>
        public static readonly Error CannotCreditUnissuedInvoice = new(
            "Invoice.CannotCreditUnissued", "A credit note can only be created for a sent, paid, or overdue invoice.");

        /// <summary>
        /// Represents an error indicating that a payment amount must be greater than zero.
        /// </summary>
        public static readonly Error InvalidPaymentAmount = new(
            "Invoice.InvalidPaymentAmount", "Payment amount must be positive.");

        /// <summary>
        /// Represents an error indicating that an invoice cannot be deleted
        /// while it is in an active state (e.g., 'Sent', 'Paid').
        /// </summary>
        public static readonly Error CannotDeleteActiveInvoice = new(
            "Invoice.CannotDeleteActive", "An active invoice cannot be deleted. It must be voided instead.");

        /// <summary>
        /// Represents an error indicating that the exchange rate must be a positive, non-zero number.
        /// </summary>
        public static readonly Error InvalidExchangeRate = new(
            "Invoice.InvalidExchangeRate", "Exchange rate must be a positive number.");

        /// <summary>
        /// Represents an error indicating that a discount amount must be zero or a positive value.
        /// </summary>
        public static readonly Error InvalidDiscountAmount = new(
            "Invoice.InvalidDiscountAmount", "Discount amount must be positive value or zero.");
    }
}
