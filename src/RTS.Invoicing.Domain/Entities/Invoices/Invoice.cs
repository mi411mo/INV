using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.Entities.Customers;
using RTS.Invoicing.Domain.Entities.Invoices.Enums;
using RTS.Invoicing.Domain.Entities.Merchants;
using RTS.Invoicing.Domain.Enums;
using RTS.Invoicing.Domain.Errors;
using RTS.Invoicing.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RTS.Invoicing.Domain.Entities.Invoices
{
    /// <summary>
    /// Core Invoice entity representing the essential invoice data structure 
    /// for any e-invoicing system integration.
    /// </summary> 
    public class Invoice : AggregateRoot<InvoiceId>
    {
        private readonly List<InvoiceItem> _invoiceItems = new();
        private readonly List<InvoiceCustomField> _invoiceCustomFields = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="Invoice"/> aggregate root.
        /// <summary>
        /// Parameterless constructor used by the ORM for entity materialization only.
        /// </summary>
        private Invoice()
            : base()
        {
            // For ORM only.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Invoice"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="invoiceNumber">The invoice number.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="issueDate">The issue date.</param>
        /// <param name="dueDate">The due date.</param>
        /// <param name="currencyCode">The currency code.</param>
        /// <summary>
        /// Initializes a new Invoice with the given identifiers, invoice number, dates, currency code, and exchange rate, and sets default state and zeroed monetary totals.
        /// </summary>
        /// <param name="id">The aggregate identifier for the invoice.</param>
        /// <param name="merchantId">The merchant that owns the invoice.</param>
        /// <param name="customerId">The customer billed by the invoice.</param>
        /// <param name="invoiceNumber">The invoice number value object.</param>
        /// <param name="issueDate">The invoice issue date.</param>
        /// <param name="dueDate">The invoice due date.</param>
        /// <param name="currencyCode">The ISO currency code used for monetary values.</param>
        /// <param name="exchangeRate">The exchange rate applicable to the invoice.</param>
        private Invoice(
            InvoiceId id,
            MerchantId merchantId,
            CustomerId customerId,
            InvoiceNumber invoiceNumber,
            DateTime issueDate,
            DateTime dueDate,
            string currencyCode,
            decimal exchangeRate)
            : base(id)
        {
            GlobalId = Guid.NewGuid();
            InvoiceNumber = invoiceNumber;
            MerchantId = merchantId;
            CustomerId = customerId;
            IssueDate = issueDate;
            DueDate = dueDate;
            CurrencyCode = currencyCode;
            ExchangeRate = exchangeRate;

            Type = InvoiceType.StandardInvoice;
            Status = InvoiceStatus.Draft;
            IsDeleted = false;

            // Initialize monetary values
            SubTotal = Money.Zero(currencyCode);
            TotalTax = Money.Zero(currencyCode);
            DiscountAmount = Money.Zero(currencyCode);
            TotalAmount = Money.Zero(currencyCode);
        }

        /// <summary>
        /// Gets or sets the global identifier for this invoice to be exposed to public.
        /// </summary>
        /// <value>The global identifier.</value>
        public Guid GlobalId { private set; get; }

        /// <summary>
        /// Gets or sets the sequential, human-readable identifier for the invoice (e.g., INV-2024-0001).
        /// </summary>
        public InvoiceNumber InvoiceNumber { private set; get; } = null!;

        /// <summary>
        /// Gets or sets the original invoice identifier.
        /// </summary>
        /// <value>The original invoice identifier.</value>
        public InvoiceId? OriginalInvoiceId { private set; get; } = null;

        /// <summary>
        /// Gets or sets the unique identifier of the merchant who issued the invoice.
        /// </summary>
        public MerchantId MerchantId { private set; get; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier of the customer receiving the invoice.
        /// </summary>
        public CustomerId CustomerId { private set; get; } = null!;

        /// <summary>
        /// Gets or sets the type of this invoice.
        /// </summary>
        /// <value>The invoice type.</value>
        public InvoiceType Type { private set; get; } = InvoiceType.StandardInvoice;

        /// <summary>
        /// Gets or sets the current status of the invoice (e.g., 'Draft', 'Paid', 'Cancelled').
        /// </summary>
        public InvoiceStatus Status { private set; get; } = InvoiceStatus.Draft;

        /// <summary>
        /// Gets or sets the date the invoice was officially issued.
        /// </summary>
        public DateTime IssueDate { private set; get; }

        /// <summary>
        /// Gets or sets the date by which the payment is due.
        /// </summary>
        public DateTime DueDate { private set; get; }

        /// <summary>
        /// Gets or sets the three-letter ISO 4217 currency code for all monetary amounts on the invoice (e.g., "YER", "SAR").
        /// </summary>
        public string CurrencyCode { private set; get; } = "YER";

        /// <summary>
        /// Gets the currency of this invoice.
        /// </summary>
        /// <value>The currency of this invoice.</value>
        public Currency Currency
            => Currency.Create(CurrencyCode).Value!;

        /// <summary>
        /// Gets or sets the subtotal amount of all invoice items before tax and discounts are applied.
        /// </summary>
        public Money SubTotal { private set; get; } = null!;

        /// <summary>
        /// Gets or sets the total tax amount applied to the invoice.
        /// </summary>
        public Money TotalTax { private set; get; } = null!;

        /// <summary>
        /// Gets or sets the total discount amount applied to the invoice.
        /// </summary>
        public Money DiscountAmount { get; private set; } = null!;

        /// <summary>
        /// Gets the amount that has been paid.
        /// </summary>
        public Money AmountPaid { private set; get; } = null!;

        /// <summary>
        /// Gets or sets the final total amount the customer must pay (TotalSub + TotalTax - Discount).
        /// </summary>
        public Money TotalAmount { private set; get; } = null!;

        /// <summary>
        /// Gets or sets the exchange rate used if the invoice currency differs from the merchant's base currency.
        /// </summary>
        public decimal ExchangeRate { private set; get; } = 0.0m;

        /// <summary>
        /// Gets or sets optional notes or terms and conditions for the invoice.
        /// </summary>
        public string? Notes { private set; get; } = null;

        /// <summary>
        /// Gets or sets a value indicating whether the invoice record has been soft-deleted.
        /// </summary>
        public bool IsDeleted { private set; get; } = false;

        /// <summary>
        /// Gets or sets the concurrency token (RowVersion) used for optimistic locking.
        /// </summary>
        public byte[] ConcurrencyToken { private set; get; } = null!;

        /// <summary>
        /// Gets or sets the navigation property to the <see cref="Merchant"/> who issued this invoice.
        /// </summary>
        public virtual Merchant Merchant { private set; get; } = null!;

        /// <summary>
        /// Gets or sets the navigation property to the <see cref="Customer"/> who is billed by this invoice.
        /// </summary>
        public virtual Customer Customer { private set; get; } = null!;

        /// <summary>
        /// Gets or sets the original invoice entity.
        /// </summary>
        /// <value>The original invoice.</value>
        public Invoice? OriginalInvoice { private set; get; } = null;

        /// <summary>
        /// Gets or sets the collection of items included in this invoice.
        /// </summary>
        public virtual IReadOnlyList<InvoiceItem> InvoiceItems => _invoiceItems.AsReadOnly();

        /// <summary>
        /// Gets or sets the collection of invoice custom fields.
        /// </summary>
        /// <value>The invoice custom fields.</value>
        public IReadOnlyList<InvoiceCustomField> InvoiceCustomFields => _invoiceCustomFields.AsReadOnly();

        /// <summary>
        /// Create a new Invoice after validating the issue/due dates and the currency.
        /// </summary>
        /// <param name="merchantId">Identifier of the merchant that owns the invoice.</param>
        /// <param name="customerId">Identifier of the customer the invoice is issued to.</param>
        /// <param name="invoiceNumber">Invoice number to assign to the new invoice.</param>
        /// <param name="issueDate">Date the invoice is issued; must not be in the future.</param>
        /// <param name="dueDate">Date the invoice is due; must not be earlier than <paramref name="issueDate"/>.</param>
        /// <param name="currency">ISO currency code for the invoice; must be a valid currency.</param>
        /// <param name="exchangeRate">Optional exchange rate for the invoice currency; defaults to 0.0.</param>
        /// <returns>A <see cref="Result{Invoice}"/> containing the created <see cref="Invoice"/> on success; a failure result with a domain error code on validation or currency failures.</returns>
        public static Result<Invoice> Create(
            MerchantId merchantId,
            CustomerId customerId,
            InvoiceNumber invoiceNumber,
            DateTime issueDate,
            DateTime dueDate,
            string currency,
            decimal exchangeRate = 0.0m)
        {
            if (issueDate > DateTime.UtcNow)
            {
                return Result.Failure<Invoice>(InvoiceErrors.IssueDateInFuture);
            }

            if (dueDate < issueDate)
            {
                return Result.Failure<Invoice>(InvoiceErrors.DueDateBeforeIssueDate);
            }

            var currencyResult = Currency.Create(currency);
            if (currencyResult.IsFailure)
            {
                return Result.Failure<Invoice>(currencyResult.Error);
            }

            return new Invoice(
                new InvoiceId(0),
                merchantId,
                customerId,
                invoiceNumber,
                issueDate,
                dueDate,
                currencyResult.Value!.Code,
                exchangeRate);
        }

        /// <summary>
        /// Creates a new, standard invoice in a valid initial state.
        /// </summary>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="invoiceNumber">The invoice number.</param>
        /// <param name="issueDate">The issue date.</param>
        /// <param name="dueDays">Due days.</param>
        /// <param name="currencyCode">The currency code.</param>
        /// <param name="exchangeRate">The exchange rate.</param>
        /// <summary>
        /// Creates a standard invoice whose due date is computed by adding <paramref name="dueDays"/> to <paramref name="issueDate"/>.
        /// </summary>
        /// <param name="dueDays">Number of days after the issue date when the invoice is due; must be zero or greater.</param>
        /// <param name="exchangeRate">Exchange rate to associate with the invoice; defaults to 0.0m.</param>
        /// <returns>A Result containing the created Invoice on success, or a failure Result (e.g., InvoiceErrors.InvalidDueDays) if validation fails.</returns>
        public static Result<Invoice> CreateStandardInvoice(
            MerchantId merchantId,
            CustomerId customerId,
            InvoiceNumber invoiceNumber,
            DateTime issueDate,
            int dueDays,
            string currencyCode,
            decimal exchangeRate = 0.0m)
        {
            if (dueDays < 0)
            {
                return Result.Failure<Invoice>(InvoiceErrors.InvalidDueDays);
            }

            var dueDate = issueDate.AddDays(dueDays);
            var invoice = new Invoice(
                new InvoiceId(0),
                merchantId,
                customerId,
                invoiceNumber,
                issueDate,
                dueDate,
                currencyCode,
                exchangeRate
                );
            return Result.Success(invoice);
        }

        // --- Business Logic Methods ---

        /// <summary>
        /// Adds an item to the invoice and recalculates totals.
        /// <summary>
        /// Adds a new invoice item to the invoice and updates invoice totals.
        /// </summary>
        /// <param name="itemOrder">Display order for the new item within the invoice.</param>
        /// <param name="description">Description of the invoice item.</param>
        /// <param name="quantity">Quantity of the item; must be greater than zero.</param>
        /// <param name="unitPrice">Unit price for the item; must be greater than or equal to zero.</param>
        /// <param name="discount">Per-item discount amount in the invoice currency (defaults to 0).</param>
        /// <param name="taxes">Optional collection of taxes to apply to the created item.</param>
        /// <returns>
        /// A Result indicating success or failure. Failure reasons include:
        /// - InvoiceErrors.NotDraft when the invoice is not in Draft status.
        /// - InvoiceItemErrors.InvalidAmount when quantity or unit price are invalid.
        /// - Any validation errors returned by InvoiceItem.Create.
        /// </returns>
        public Result AddItem(
            short itemOrder,
            string description,
            int quantity,
            decimal unitPrice,
            decimal discount = 0,
            IEnumerable<Tax>? taxes = null)
        {
            if (Status != InvoiceStatus.Draft)
            {
                return Result.Failure(InvoiceErrors.NotDraft);
            }

            if (quantity <= 0 || unitPrice < 0)
            {
                return Result.Failure(InvoiceItemErrors.InvalidAmount);
            }

            var itemResult = InvoiceItem.Create(
                Id,
                itemOrder,
                description,
                quantity,
                unitPrice,
                discount,
                SubTotal.Currency.Code);
            if (itemResult.IsFailure)
            {
                return Result.Failure(itemResult.Error);
            }

            var invoiceItem = itemResult.Value!;

            if (taxes is not null)
            {
                foreach (var tax in taxes)
                {
                    invoiceItem.ApplyTax(tax, CurrencyCode);
                }
            }

            _invoiceItems.Add(invoiceItem);
            RecalculateTotals();

            return Result.Success();
        }

        /// <summary>
        /// Removes the invoice item from the.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <summary>
        /// Removes the invoice item with the specified identifier from the invoice.
        /// </summary>
        /// <param name="itemId">Identifier of the invoice item to remove.</param>
        /// <returns>A <see cref="Result"/> indicating success, or a failure result with <see cref="InvoiceErrors.NotDraft"/> if the invoice is not in Draft status, or <see cref="InvoiceErrors.ItemNotFound"/> if no matching item exists.</returns>
        public Result RemoveItem(InvoiceItemId itemId)
        {
            if (Status != InvoiceStatus.Draft)
            {
                return Result.Failure(InvoiceErrors.NotDraft);
            }

            var itemToRemove = _invoiceItems.FirstOrDefault(item => item.Id == itemId);
            if (itemToRemove is null)
            {
                return Result.Failure(InvoiceErrors.ItemNotFound);
            }

            _invoiceItems.Remove(itemToRemove);

            RecalculateTotals();

            return Result.Success();
        }

        /// <summary>
        /// Updates the invoice item information.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="itemOrder">The item order.</param>
        /// <param name="description">The description.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="unitPrice">The unit price.</param>
        /// <param name="discountAmount">The discount amount.</param>
        /// <summary>
        /// Updates the specified invoice item with the provided order, description, quantity, unit price, and discount when the invoice is in Draft.
        /// </summary>
        /// <param name="itemId">Identifier of the invoice item to update.</param>
        /// <param name="itemOrder">New display/order position for the item.</param>
        /// <param name="description">New description for the item.</param>
        /// <param name="quantity">New quantity for the item (must be greater than zero for a valid update).</param>
        /// <param name="unitPrice">New unit price for the item.</param>
        /// <param name="discountAmount">New discount amount applied to the item.</param>
        /// <returns>`Success` if the item was updated (and totals were recalculated); `Failure` with `InvoiceErrors.NotDraft` if the invoice is not in Draft, `InvoiceErrors.ItemNotFound` if the item does not exist, or the underlying item update failure if the item validation fails.</returns>
        public Result UpdateItem(
            InvoiceItemId itemId,
            short itemOrder,
            string description,
            int quantity,
            decimal unitPrice,
            decimal discountAmount)
        {
            if (Status != InvoiceStatus.Draft)
            {
                return Result.Failure(InvoiceErrors.NotDraft);
            }

            var itemToUpdate = _invoiceItems.FirstOrDefault(item => item.Id == itemId);
            if (itemToUpdate is null)
            {
                return Result.Failure(InvoiceErrors.ItemNotFound);
            }

            var updateResult = itemToUpdate.UpdateDetails(itemOrder, description, quantity, unitPrice, discountAmount);

            if (updateResult.IsSuccess)
            {
                RecalculateTotals();
            }

            return updateResult;
        }

        /// <summary>
        /// Applies a tax or more to a specific item in this invoice.
        /// </summary>
        /// <param name="itemId">The invoice item identifier.</param>
        /// <param name="taxes">Taxes to be applied.</param>
        /// <summary>
        /// Applies the specified taxes to a single invoice item and recalculates invoice totals.
        /// </summary>
        /// <param name="itemId">The identifier of the invoice item to which taxes will be applied.</param>
        /// <param name="taxes">The taxes to apply to the specified item.</param>
        /// <returns>A Result indicating success, or a failure with
        /// InvoiceErrors.NotDraft if the invoice is not in Draft status, or
        /// InvoiceErrors.ItemNotFound if no item with the given id exists.</returns>
        public Result ApplyTaxForInvoiceItem(InvoiceItemId itemId, IEnumerable<Tax> taxes)
        {
            if (Status != InvoiceStatus.Draft)
            {
                return Result.Failure(InvoiceErrors.NotDraft);
            }

            var itemToApplyToOn = _invoiceItems.FirstOrDefault(item => item.Id == itemId);
            if (itemToApplyToOn is null)
            {
                return Result.Failure(InvoiceErrors.ItemNotFound);
            }

            foreach (var tax in taxes)
            {
                itemToApplyToOn.ApplyTax(tax, CurrencyCode);
            }

            RecalculateTotals();

            return Result.Success();
        }

        /// <summary>
        /// Applies a discount to the entire invoice.
        /// <summary>
        /// Sets the invoice-level discount to the specified non-negative amount (in the invoice currency) and recalculates totals.
        /// </summary>
        /// <param name="discountAmount">The discount amount in the invoice currency; must be greater than or equal to 0.</param>
        /// <returns>`Result` indicating success, or failure with `InvoiceErrors.NotDraft` if the invoice is not in Draft state, or `InvoiceErrors.InvalidDiscountAmount` if the amount is negative.</returns>
        public Result ApplyInvoiceDiscount(decimal discountAmount)
        {
            if (Status != InvoiceStatus.Draft)
            {
                return Result.Failure(InvoiceErrors.NotDraft);
            }

            if (discountAmount < 0)
            {
                return Result.Failure(InvoiceErrors.InvalidDiscountAmount);
            }

            DiscountAmount = new Money(discountAmount, Currency);
            RecalculateTotals();
            return Result.Success();
        }

        /// <summary>
        /// Updates the invoice notes only if it is Draft.
        /// </summary>
        /// <summary>
        /// Update the invoice's notes when the invoice is in Draft status.
        /// </summary>
        /// <param name="notes">The new notes text; may be null to clear existing notes.</param>
        public void UpdateNotes(string? notes)
        {
            if (Status == InvoiceStatus.Draft)
            {
                Notes = notes;
            }
        }

        /// <summary>
        /// Marks the invoice as sent, finalizing it for payment.
        /// <summary>
        /// Marks the invoice as sent and records the send time when it is in Draft and has at least one item.
        /// </summary>
        /// <returns>`Result` indicating the outcome: success when the invoice was moved to Sent and IssueDate set to the current UTC time; `Failure(InvoiceErrors.NotDraft)` if the invoice is not in Draft; `Failure(InvoiceErrors.CannotSendEmptyInvoice)` if the invoice has no items.</returns>
        public Result MarkAsSent()
        {
            if (Status != InvoiceStatus.Draft)
            {
                return Result.Failure(InvoiceErrors.NotDraft);
            }

            if (!_invoiceItems.Any())
            {
                return Result.Failure(InvoiceErrors.CannotSendEmptyInvoice);
            }

            Status = InvoiceStatus.Sent;
            IssueDate = DateTime.UtcNow;
            return Result.Success();
        }

        /// <summary>
        /// Checks the invoice status against the current date and marks it as overdue if necessary.
        /// This should be called by a background process or application service.
        /// <summary>
        /// Mark the invoice as overdue if the provided date is after the due date and the invoice is in a sent or partially paid state.
        /// </summary>
        /// <param name="currentDate">The date to compare against the invoice's due date.</param>
        /// <returns>A success <see cref="Result"/>.</returns>
        public Result MarkAsOverdue(DateTime currentDate)
        {
            if (currentDate <= DueDate)
            {
                return Result.Success();
            }

            if (Status is InvoiceStatus.Sent or InvoiceStatus.PartiallyPaid)
            {
                Status = InvoiceStatus.Overdue;
            }

            return Result.Success();
        }

        /// <summary>
        /// Applies a payment to the invoice, updating the AmountPaid and Status.
        /// This replaces the simple MarkAsPaid methods.
        /// <summary>
        /// Applies a payment to the invoice and updates AmountPaid and the invoice Status accordingly.
        /// </summary>
        /// <param name="paymentAmount">The payment to apply in the invoice currency; amount must be greater than zero.</param>
        /// <returns>`Success` when the payment is applied; `Failure(InvoiceErrors.InvalidStateForPayment)` if the invoice is in Draft, Paid, or Void state, or `Failure(InvoiceErrors.InvalidPaymentAmount)` if the payment amount is not greater than zero.</returns>
        public Result ApplyPayment(Money paymentAmount)
        {
            if (Status is InvoiceStatus.Draft or InvoiceStatus.Paid or InvoiceStatus.Void)
            {
                return Result.Failure(InvoiceErrors.InvalidStateForPayment);
            }

            if (paymentAmount.Amount <= 0)
            {
                return Result.Failure(InvoiceErrors.InvalidPaymentAmount);
            }

            AmountPaid = new Money(AmountPaid.Amount + paymentAmount.Amount, Currency);

            // TODO: Revise this.
            if (AmountPaid.Amount >= TotalAmount.Amount - 0.0001m)
            {
                Status = InvoiceStatus.Paid;
            }
            else
            {
                Status = InvoiceStatus.PartiallyPaid;
            }

            return Result.Success();
        }

        /// <summary>
        /// Voids the invoice, making it non-payable.
        /// <summary>
        /// Marks the invoice as void and soft-deletes it unless the invoice is paid or partially paid.
        /// </summary>
        /// <returns>`Result.Success()` when the invoice is marked void; `Result.Failure(InvoiceErrors.CannotVoidPaidInvoice)` if the invoice is Paid or PartiallyPaid.</returns>
        public Result Void()
        {
            if (Status is InvoiceStatus.Paid or InvoiceStatus.PartiallyPaid)
            {
                return Result.Failure(InvoiceErrors.CannotVoidPaidInvoice);
            }

            Status = InvoiceStatus.Void;
            IsDeleted = true;
            return Result.Success();
        }

        /// <summary>
        /// Sets or updates the value for a custom field on this invoice.
        /// The aggregate root is responsible for enforcing type validation based on the CustomField definition.
        /// </summary>
        /// <param name="customField">The CustomField definition (retrieved from a repository).</param>
        /// <summary>
        /// Sets or updates the value of a custom field on this invoice when the invoice is in Draft state.
        /// </summary>
        /// <param name="customField">The custom field definition to apply; must belong to the same merchant as the invoice.</param>
        /// <param name="value">The value to assign to the custom field, represented as a string.</param>
        /// <returns>
        /// A <see cref="Result"/> indicating success or failure. Possible failures include:
        /// <list type="bullet">
        /// <item><description><see cref="InvoiceErrors.NotDraft"/> if the invoice is not in Draft state.</description></item>
        /// <item><description><see cref="CustomFieldErrors.MerchantMismatch"/> if the custom field's merchant does not match the invoice's merchant.</description></item>
        /// <item><description><see cref="InvoiceCustomFieldErrors.InvalidValueType"/> if the value is not valid for the custom field's type.</description></item>
        /// <item><description>Any failure returned by creating or updating the underlying <see cref="InvoiceCustomField"/>.</description></item>
        /// </list>
        /// </returns>
        public Result SetCustomFieldValue(CustomField customField, string value)
        {
            if (Status != InvoiceStatus.Draft)
            {
                return Result.Failure(InvoiceErrors.NotDraft);
            }

            if (customField.MerchantId != this.MerchantId)
            {
                return Result.Failure(CustomFieldErrors.MerchantMismatch);
            }

            if (!IsValueValidForType(value, customField.Type))
            {
                return Result.Failure(InvoiceCustomFieldErrors.InvalidValueType);
            }

            var existingField = _invoiceCustomFields.FirstOrDefault(f => f.CustomFieldId == customField.Id);

            if (existingField is not null)
            {
                return existingField.UpdateValue(value);
            }

            var newFieldResult = InvoiceCustomField.Create(this.Id, customField.Id, value);
            if (newFieldResult.IsFailure)
            {
                return newFieldResult;
            }

            _invoiceCustomFields.Add(newFieldResult.Value!);

            return Result.Success();
        }

        /// <summary>
        /// Updates the exchange rate of a Draft invoice.
        /// </summary>
        /// <param name="newRate">The new exchange rate.</param>
        /// <summary>
        /// Updates the invoice's exchange rate when the invoice is in the Draft state.
        /// </summary>
        /// <param name="newRate">The new exchange rate to apply; must be greater than zero.</param>
        /// <returns>A Result indicating success, or failure with `InvoiceErrors.NotDraft` if the invoice is not Draft, or `InvoiceErrors.InvalidExchangeRate` if `newRate` is less than or equal to zero.</returns>
        public Result UpdateExchangeRate(decimal newRate)
        {
            if (Status != InvoiceStatus.Draft)
            {
                return Result.Failure(InvoiceErrors.NotDraft);
            }

            if (newRate <= 0)
            {
                return Result.Failure(InvoiceErrors.InvalidExchangeRate);
            }

            ExchangeRate = newRate;
            return Result.Success();
        }

        // --- Private Helper Methods ---

        /// <summary>
        /// Recalculates Subtotal, TotalTax, and TotalAmount based on current items and taxes.
        /// This is the single source of truth for invoice calculations.
        /// <summary>
        /// Recalculates monetary totals for the invoice based on current items, their taxes, and the invoice discount.
        /// </summary>
        /// <remarks>
        /// Updates SubTotal, TotalTax, and TotalAmount using the invoice's currency and current invoice items and discounts.
        /// </remarks>
        private void RecalculateTotals()
        {
            var currency = SubTotal.Currency;

            var calculatedSubtotal = _invoiceItems.Sum(item => item.TotalPrice.Amount);

            SubTotal = new Money(calculatedSubtotal, currency);

            var calculatedTotalTax = _invoiceItems
                .SelectMany(item => item.Taxes)
                .Sum(tax => tax.TaxAmount.Amount);
            TotalTax = new Money(calculatedTotalTax, Currency);

            var calculatedTotalAmount = SubTotal.Amount + TotalTax.Amount - DiscountAmount.Amount;
            TotalAmount = new Money(calculatedTotalAmount, Currency);
        }

        /// <summary>
        /// Private helper to validate the string value against the required data type.
        /// <summary>
        /// Determine whether a custom field string value conforms to the specified CustomFieldsTypes.
        /// </summary>
        /// <param name="value">The string representation of the custom field value to validate; empty or null is considered valid.</param>
        /// <param name="type">The expected custom field type to validate against.</param>
        /// <returns>`true` if the value is null or empty or successfully parses to the expected type; `false` otherwise.</returns>
        private static bool IsValueValidForType(string value, CustomFieldsTypes type)
        {
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }

            switch (type)
            {
                case CustomFieldsTypes.Number:
                    return decimal.TryParse(value, out _);

                case CustomFieldsTypes.Text:
                    return true;

                case CustomFieldsTypes.Date:
                case CustomFieldsTypes.DateTime:
                    return DateTime.TryParse(value, out _);

                case CustomFieldsTypes.BinaryValue:
                    return bool.TryParse(value, out _);

                default:
                    return false;
            }
        }
    }
}