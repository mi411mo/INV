using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.Aggregates.Merchants;
using RTS.Invoicing.Domain.Errors;
using RTS.Invoicing.Domain.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace RTS.Invoicing.Domain.Aggregates.Invoices
{
    /// <summary>
    /// Represents a single line item within an invoice, detailing the quantity, pricing, and description of a service or product.
    /// </summary>
    public class InvoiceItem : Entity<InvoiceItemId>
    {
        /// <summary>
        /// Represents the collection of taxes applied to an invoice item.
        /// </summary>
        /// <remarks>This field stores the taxes associated with an invoice item. It is initialized as an
        /// empty list and is intended for internal use within the class to manage tax-related calculations or
        /// operations.</remarks>
        private readonly List<InvoiceItemTax> _taxes = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="InvoiceItem" /> class.
        /// </summary>
        private InvoiceItem()
            : base()
        {
            // For ORM Only.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvoiceItem"/> class.
        /// </summary>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <param name="itemOrder">The item order.</param>
        /// <param name="description">The description.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="unitPrice">The unit price.</param>
        /// <param name="discountAmount">The discount amount.</param>
        private InvoiceItem(
            InvoiceId invoiceId,
            short itemOrder,
            string description,
            int quantity,
            Money unitPrice,
            Money? discountAmount = null)
        {
            InvoiceId = invoiceId;
            ItemOrder = itemOrder;
            Description = description;
            Quantity = quantity;
            UnitPrice = unitPrice;
            DiscountAmount = discountAmount ?? Money.Zero("YER");

            RecalculateTotals();
        }

        /// <summary>
        /// Gets or sets the foreign key referencing the parent Invoice entity.
        /// </summary>
        public InvoiceId InvoiceId { private set; get; } = null!;

        /// <summary>
        /// Gets or sets the item order.
        /// This can help to order the items in a specific order.
        /// </summary>
        /// <value>The item order in the invoice.</value>
        public short ItemOrder { private set; get; }

        /// <summary>
        /// Gets the detailed, customer-facing description of the service or product.
        /// </summary>
        /// <value>
        /// A string containing the description. This value cannot be null.
        /// </value>
        /// <remarks>
        /// This text is displayed directly on the invoice line item. It should be clear
        /// and concise for the recipient to easily understand the charge.
        /// </remarks>
        public string Description { private set; get; } = string.Empty;

        /// <summary>
        /// Gets or sets the quantity of the item purchased or service rendered.
        /// </summary>
        public int Quantity { private set; get; }

        /// <summary>
        /// Gets or sets the price per unit of the item, excluding any taxes.
        /// </summary>
        public Money UnitPrice { private set; get; } = null!;

        /// <summary>
        /// Gets or sets the calculated total price for this line item (Quantity * UnitPrice), excluding tax.
        /// </summary>
        public Money TotalPrice { private set; get; } = null!;

        /// <summary>
        /// Gets or sets the total discount amount applied to this invoice line item.
        /// This enables adding discounts per item for the invoice.
        /// </summary>
        public Money DiscountAmount { private set; get; } = null!;

        /// <summary>
        /// Gets the collection of taxes associated with the invoice item.
        /// </summary>
        public IReadOnlyList<InvoiceItemTax> Taxes => _taxes.AsReadOnly();

        /// <summary>
        /// Creates the specified invoice.
        /// </summary>
        /// <param name="invoiceId">The invoice identifier to link the item with.</param>
        /// <param name="itemOrder">The invoice item order in the list.</param>
        /// <param name="description">The description of the item (Display name).</param>
        /// <param name="quantity">The quantity of this item.</param>
        /// <param name="unitPrice">The item unit price.</param>
        /// <param name="discountAmount">The discount amount.</param>
        /// <param name="currencyCode">The currency code.</param>
        /// <returns></returns>
        public static Result<InvoiceItem> Create(
            InvoiceId invoiceId,
            short itemOrder,
            string description,
            int quantity,
            decimal unitPrice,
            decimal discountAmount,
            string currencyCode
            )
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                return Result.Failure<InvoiceItem>(InvoiceItemErrors.InvalidDescription);
            }

            if (quantity <= 0)
            {
                return Result.Failure<InvoiceItem>(InvoiceItemErrors.InvalidQuantity);
            }

            if (unitPrice <= 0)
            {
                return Result.Failure<InvoiceItem>(InvoiceItemErrors.InvalidUnitPrice);
            }

            var currencyResult = Currency.Create(currencyCode);
            if (currencyResult.IsFailure)
            {
                return Result.Failure<InvoiceItem>(currencyResult.Error);
            }

            var unitPriceMoney = new Money(unitPrice, currencyResult.Value!);
            var discountMoney = new Money(discountAmount, currencyResult.Value!);

            if ((quantity * unitPrice) < discountAmount)
            {
                return Result.Failure<InvoiceItem>(InvoiceItemErrors.DiscountExceedsTotal);
            }

            var invoiceItem = new InvoiceItem(
                invoiceId,
                itemOrder,
                description,
                quantity,
                unitPriceMoney,
                discountMoney);
            return Result.Success(invoiceItem);
        }

        /// <summary>
        /// Updates the invoice item details.
        /// </summary>
        /// <param name="itemOrder">The item order.</param>
        /// <param name="description">The description.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="unitPrice">The unit price.</param>
        /// <param name="discountAmount">The discount amount.</param>
        /// <returns></returns>
        internal Result UpdateDetails(short itemOrder, string description, int quantity, decimal unitPrice, decimal discountAmount)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                return Result.Failure(InvoiceItemErrors.InvalidDescription);
            }

            if (quantity <= 0)
            {
                return Result.Failure(InvoiceItemErrors.InvalidQuantity);
            }

            if (unitPrice < 0)
            {
                return Result.Failure(InvoiceItemErrors.InvalidUnitPrice);
            }

            if (discountAmount < 0)
            {
                return Result.Failure(InvoiceItemErrors.InvalidDiscount);
            }

            if ((quantity * unitPrice) < discountAmount)
            {
                return Result.Failure(InvoiceItemErrors.DiscountExceedsTotal);
            }

            ItemOrder = itemOrder;
            Description = description;
            Quantity = quantity;
            UnitPrice = new Money(unitPrice, UnitPrice.Currency);
            DiscountAmount = new Money(discountAmount, UnitPrice.Currency);

            RecalculateTotals();

            return Result.Success();
        }

        /// <summary>
        /// Updates the item order.
        /// </summary>
        /// <param name="itemOrder">The item order.</param>
        public void UpdateItemOrder(short itemOrder)
        {
            ItemOrder = itemOrder;
        }

        /// <summary>
        /// Applies a tax to the item.
        /// </summary>
        /// <param name="tax">The tax object.</param>
        /// <param name="currency">The currency.</param>
        public Result ApplyTax(Tax tax, string currency)
        {
            if (_taxes.Any(t => t.TaxId == tax.Id))
            {
                return Result.Failure(InvoiceItemErrors.DuplicatedTax);
            }

            var invoiceItemTaxResult = InvoiceItemTax.Create(this, tax, currency);
            if (invoiceItemTaxResult.IsFailure)
            {
                return invoiceItemTaxResult;
            }

            _taxes.Add(invoiceItemTaxResult.Value!);
            RecalculateTotals();

            return Result.Success();
        }

        /// <summary>
        /// Applies a discount to the item.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public void ApplyDiscount(decimal amount)
        {
            if (amount <= 0)
            {
                // No need to perform anything when discount amount is less or equal to 0.
                return;
            }

            var newDiscountAmount = DiscountAmount.Amount + amount;
            DiscountAmount = new Money(newDiscountAmount, DiscountAmount.Currency);

            RecalculateTotals();
        }

        /// <summary>
        /// Recalculates the totals.
        /// </summary>
        private void RecalculateTotals()
        {
            TotalPrice = new((Quantity * UnitPrice.Amount) - DiscountAmount.Amount, UnitPrice.Currency);
        }
    }
}
