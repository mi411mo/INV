using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.Aggregates.Invoices;
using RTS.Invoicing.Domain.Aggregates.Payments.Enums;
using RTS.Invoicing.Domain.Errors;
using RTS.Invoicing.Domain.ValueObjects;
using System;

namespace RTS.Invoicing.Domain.Aggregates.Payments
{
    /// <summary>
    /// Represents a recorded payment transaction against a specific invoice.
    /// </summary>
    public class Payment : AggregateRoot<PaymentId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Payment"/> entity.
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
        /// <param name="transactionReference">The transaction reference.</param>
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
        /// Creates a new <see cref="Payment"/> instance.
        /// </summary>
        /// <param name="invoiceId">The ID of the invoice this payment is for.</param>
        /// <param name="amount">The monetary amount of the payment.</param>
        /// <param name="currencyCode">The ISO 4217 currency code for the amount.</param>
        /// <param name="type">The type of payment (e.g., Payment or Refund).</param>
        /// <param name="method">The payment method (e.g., "Credit Card", "Bank Transfer").</param>
        /// <param name="transactionReference">The unique reference ID from the payment processor.</param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing the new <see cref="Payment"/> instance if creation is successful,
        /// or an <see cref="Error"/> if the amount, reference, or currency is invalid.
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
        /// <param name="notes">The notes.</param>
        public void UpdateNotes(string? notes)
        {
            Notes = notes;
        }

        /// <summary>
        /// Marks as succeeded.
        /// </summary>
        /// <returns></returns>
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
        /// <returns></returns>
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
