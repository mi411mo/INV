using RTS.Invoicing.Domain.Common;

namespace RTS.Invoicing.Domain.Errors
{
    public static class InvoiceErrors
    {
        public static readonly Error NotDraft = new(
            "Invoice.NotDraft", "The action can only be performed on a draft invoice.");

        public static readonly Error NoItems = new(
            "Invoice.NoItems", "Cannot send an invoice with no items.");

        public static readonly Error IsPaid = new(
            "Invoice.IsPaid", "Cannot cancel an invoice that has received payments. Consider a credit note instead.");

        public static readonly Error InvalidStatusForPayment = new(
            "Invoice.InvalidStatusForPayment", "Payment cannot be applied to a paid or cancelled invoice.");

        public static readonly Error InvalidDueDays = new(
           "Invoice.InvalidDueDays", "Due days cannot be negative.");

        public static readonly Error InvalidNumber = new(
            "Invoice.InvalidNumber", "Invoice number cannot be empty.");

        public static readonly Error IssueDateInFuture = new(
            "Invoice.IssueDateInFuture", "Issue date cannot be in the future.");

        public static readonly Error DueDateBeforeIssueDate = new(
            "Invoice.DueDateBeforeIssueDate", "Due date cannot be before the issue date.");

        public static readonly Error CannotSendEmptyInvoice = new(
            "Invoice.CannotSendEmpty", "Cannot send an invoice with no items.");

        public static readonly Error ItemNotFound = new(
            "Invoice.ItemNotFound", "The specified invoice item was not found.");

        public static readonly Error InvalidStateForPayment = new(
            "Invoice.InvalidStateForPayment", "Invoice is not in a state that can be marked as paid.");

        public static readonly Error InvalidStateForPartialPayment = new(
            "Invoice.InvalidStateForPartialPayment", "Invoice is not in a state that can be partially paid.");

        public static readonly Error CannotVoidPaidInvoice = new(
            "Invoice.CannotVoidPaid", "A paid or partially paid invoice cannot be voided. A credit note should be issued instead.");

        public static readonly Error CannotCreditUnissuedInvoice = new(
            "Invoice.CannotCreditUnissued", "A credit note can only be created for a sent, paid, or overdue invoice.");

        public static readonly Error InvalidPaymentAmount = new(
            "Invoice.InvalidPaymentAmount", "Payment amount must be positive.");

        public static readonly Error CannotDeleteActiveInvoice = new(
            "Invoice.CannotDeleteActive", "An active invoice cannot be deleted. It must be voided instead.");

        public static readonly Error InvalidExchangeRate = new(
            "Invoice.InvalidExchangeRate", "Exchange rate must be a positive number.");

        public static readonly Error InvalidDiscountAmount = new(
            "Invoice.InvalidDiscountAmount", "Discount amount must be positive value or zero.");
    }
}
