using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.Entities.Invoices;
using RTS.Invoicing.Domain.Entities.Payments.Enums;
using RTS.Invoicing.Domain.Errors;
using RTS.Invoicing.Domain.ValueObjects;
using System;

namespace RTS.Invoicing.Domain.Entities.Payments
{
    /// <summary>
    /// Represents a recorded payment transaction against a specific invoice.
    /// </summary>
    public class Payment : AggregateRoot<PaymentId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Payment"/> entity.
        /// <summary>
        /// Initializes a new instance of the <see cref="Payment"/> class for ORM/serialization.
        /// </summary>
        private Payment()
            : base()
        {
            // For ORM only.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Payment"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <param name="amount">The payment amount.</param>
        /// <param name="method">The payment method.</param>
        /// <param name="currencyCode">The currency code.</param>
        /// <param name="date">The date of the payment.</param>
        /// <param name="type">The payment type.</param>
        /// <summary>
        /// Initializes a new <see cref="Payment"/> with the specified identifiers, monetary values, metadata, and sets <see cref="Status"/> to <see cref="PaymentStatus.Pending"/>.
        /// </summary>
        /// <param name="id">The payment identifier.</param>
        /// <param name="invoiceId">Identifier of the invoice this payment is applied to.</param>
        /// <param name="amount">The monetary amount of the payment.</param>
        /// <param name="method">The payment method or provider description.</param>
        /// <param name="currencyCode">The ISO currency code for the payment amount.</param>
        /// <param name="date">The date and time the payment was recorded.</param>
        /// <param name="type">The payment type (e.g., payment, refund).</param>
        /// <param name="transactionReference">The processor or bank transaction reference.</param>
        private Payment(
            PaymentId id,
            InvoiceId invoiceId,
            Money amount,
            string method,
            string currencyCode,
            DateTime date,
            PaymentType type,
            string transactionReference)
            : base(id)
        {
            InvoiceId = invoiceId;
            Amount = amount;
            Method = method;
            CurrencyCode = currencyCode;
            Date = date;
            Type = type;
            Status = PaymentStatus.Pending;
            TransactionReference = transactionReference;
        }

        /// <summary>
        /// Gets or sets the foreign key referencing the parent Invoice entity.
        /// </summary>
        public InvoiceId InvoiceId { private set; get; } = null!;

        /// <summary>
        /// Gets or sets the monetary amount of the payment recorded.
        /// </summary>
        public Money Amount { private set; get; } = null!;

        /// <summary>
        /// Gets or sets the method used for the payment (e.g., "Credit Card", "Bank Transfer", "Cash").
        /// </summary>
        public string Method { private set; get; } = string.Empty; // TODO: Should I make the payment method enum or not?
        // NOTE: This should ideally be an Enum for a closed set of valid values?

        /// <summary>
        /// Gets or sets the three-letter ISO 4217 currency code for the payment amount (e.g., "YER", "USD", "SAR").
        /// </summary>
        public string CurrencyCode { private set; get; } = "YER";

        /// <summary>
        /// Gets or sets the date and time when the payment was received or processed.
        /// </summary>
        public DateTime Date { private set; get; }

        /// <summary>
        /// Gets or sets the type of payment.
        /// </summary>
        public PaymentType Type { private set; get; } = PaymentType.Payment;

        /// <summary>
        /// Gets or sets the current status of the payment (e.g., 'Pending', 'Succeeded', 'Failed').
        /// </summary>
        public PaymentStatus Status { private set; get; } = PaymentStatus.Pending;

        /// <summary>
        /// Gets or sets the unique reference number provided by the payment processor or bank.
        /// </summary>
        public string TransactionReference { private set; get; } = string.Empty;

        /// <summary>
        /// Gets or sets any optional notes related to the payment.
        /// </summary>
        public string? Notes { private set; get; } = null;

        /// <summary>
        /// Gets or sets the concurrency token (RowVersion) used for optimistic locking.
        /// </summary>
        public byte[] ConcurrencyToken { private set; get; } = null!;

        /// <summary>
        /// Creates a new Payment for the specified invoice after validating amount, currency, and transaction reference.
        /// </summary>
        /// <param name="invoiceId">Identifier of the invoice the payment applies to.</param>
        /// <param name="amount">Monetary amount; must be greater than zero when <paramref name="type"/> is <see cref="PaymentType.Payment"/>.</param>
        /// <param name="currencyCode">ISO currency code to validate and use for the payment amount.</param>
        /// <param name="type">The kind of payment being recorded.</param>
        /// <param name="method">Human-readable payment method description (e.g., card, bank transfer).</param>
        /// <param name="transactionReference">Processor or bank transaction reference; must be non-empty.</param>
        /// <returns>
        /// A Result containing the created <see cref="Payment"/> on success; on failure contains an <see cref="Error"/> describing either an invalid amount, an empty transaction reference, or a currency validation error.
        /// </returns>
        public static Result<Payment> Create(
            InvoiceId invoiceId,
            decimal amount,
            string currencyCode,
            PaymentType type,
            string method,
            string transactionReference)
        {
            if (amount <= 0 && type == PaymentType.Payment)
            {
                return Result.Failure<Payment>(new Error("Payment.Amount.Invalid", "Payment amount must be positive."));
            }

            if (string.IsNullOrWhiteSpace(transactionReference))
            {
                return Result.Failure<Payment>(new Error("Payment.Reference.Empty", "Transaction reference cannot be empty."));
            }

            var currencyResult = Currency.Create(currencyCode);
            if (currencyResult.IsFailure)
            {
                return Result.Failure<Payment>(currencyResult.Error);
            }

            var moneyAmount = new Money(amount, currencyResult.Value!);

            var payment = new Payment(
                new PaymentId(0),
                invoiceId,
                moneyAmount,
                method,
                currencyCode,
                DateTime.UtcNow,
                type,
                transactionReference);

            return Result.Success(payment);
        }

        /// <summary>
        /// Updates the payment notes.
        /// </summary>
        /// <summary>
        /// Sets or replaces the payment's notes.
        /// </summary>
        /// <param name="notes">The notes to store; pass <c>null</c> to clear existing notes.</param>
        public void UpdateNotes(string? notes)
        {
            Notes = notes;
        }

        /// <summary>
        /// Marks as succeeded.
        /// </summary>
        /// <summary>
        /// Mark the payment as succeeded when it is currently pending.
        /// </summary>
        /// <returns>A successful Result when the payment status is set to Succeeded; otherwise a failed Result containing PaymentErrors.AlreadyProcessed.</returns>
        public Result MarkAsSucceeded()
        {
            if (Status != PaymentStatus.Pending)
            {
                return Result.Failure(PaymentErrors.AlreadyProcessed);
            }

            Status = PaymentStatus.Succeeded;
            return Result.Success();
        }

        /// <summary>
        /// Marks as failed.
        /// </summary>
        /// <param name="reason">The reason of failure.</param>
        /// <summary>
        /// Marks the payment as failed and records an optional failure reason.
        /// </summary>
        /// <param name="reason">Optional text describing why the payment failed; appended to existing Notes if present.</param>
        /// <returns>The result indicating success when the payment was transitioned to Failed; otherwise a failure with PaymentErrors.AlreadyProcessed.</returns>
        public Result MarkAsFailed(string? reason = null)
        {
            if (Status != PaymentStatus.Pending)
            {
                return Result.Failure(PaymentErrors.AlreadyProcessed);
            }

            Status = PaymentStatus.Failed;
            Notes = string.IsNullOrWhiteSpace(Notes) ? reason : $"{Notes}\nFailure Reason: {reason ?? "None"}";
            return Result.Success();
        }
    }
}