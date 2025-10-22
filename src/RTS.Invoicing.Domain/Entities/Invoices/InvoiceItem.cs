using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.Entities.Merchants;
using RTS.Invoicing.Domain.Errors;
using RTS.Invoicing.Domain.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace RTS.Invoicing.Domain.Entities.Invoices
{
    /// <summary>
    /// Represents a single line item within an invoice, detailing the quantity, pricing, and description of a service or product.
    /// </summary>
    public class InvoiceItem : Entity<InvoiceItemId>
    {
        private readonly List<InvoiceItemTax> _taxes = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="InvoiceItem" /> class.
        /// <summary>
        /// Initializes a new instance of InvoiceItem for ORM and deserialization purposes.
        /// </summary>
        /// <remarks>
        /// This constructor is intended for use by object-relational mappers and serialization; do not use directly in application code.
        /// </remarks>
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
        /// <summary>
        /// Initializes a new InvoiceItem with the specified invoice association, order, description, quantity, unit price, and optional discount.
        /// </summary>
        /// <param name="discountAmount">The discount amount to apply; if null, defaults to zero using the "YER" currency.</param>
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
        /// <summary>
        /// Creates and validates a new InvoiceItem instance.
        /// </summary>
        /// <param name="discountAmount">Total discount amount to apply to the item (in the specified currency).</param>
        /// <param name="currencyCode">ISO currency code used for the unit price and discount amounts.</param>
        /// <returns>A successful Result containing the created InvoiceItem, or a failed Result containing a specific error when validation fails (e.g. InvalidDescription, InvalidQuantity, InvalidUnitPrice, DiscountExceedsTotal) or when the currency cannot be created.</returns>
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
        /// <summary>
        /// Update core item details (order, description, quantity, unit price, discount) and recalculate totals.
        /// </summary>
        /// <param name="itemOrder">Display/order position of the item within the invoice.</param>
        /// <param name="description">Customer-facing description of the item.</param>
        /// <param name="quantity">Quantity of the item; must be greater than zero.</param>
        /// <param name="unitPrice">Unit price amount in the item's current currency; must be greater than or equal to zero.</param>
        /// <param name="discountAmount">Discount amount to apply to the item in the item's current currency; must be greater than or equal to zero and not exceed quantity * unitPrice.</param>
        /// <returns>`Success` when the update and recalculation complete; otherwise a failure `Result` containing the specific validation error.</returns>
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
        /// <summary>
        /// Sets the display/order position of this invoice item.
        /// </summary>
        /// <param name="itemOrder">The new display position for the item within the invoice.</param>
        public void UpdateItemOrder(short itemOrder)
        {
            ItemOrder = itemOrder;
        }

        /// <summary>
        /// Applies a tax to the item.
        /// </summary>
        /// <param name="tax">The tax object.</param>
        /// <summary>
        /// Applies a tax to the invoice item using the specified currency.
        /// </summary>
        /// <param name="tax">The tax to apply.</param>
        /// <param name="currency">The ISO currency code used when creating the invoice item tax.</param>
        /// <returns>`Result.Success()` if the tax was applied and the item's totals were updated; otherwise a failure `Result` — for example when the tax is already applied or when creating the invoice item tax fails.</returns>
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
        /// <summary>
        /// Increases the item's discount by the specified positive amount and updates totals.
        /// </summary>
        /// <param name="amount">Additional discount amount to add; if less than or equal to zero, the method does nothing. The existing currency is preserved.</param>
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
        /// <summary>
        /// Recalculates the item's TotalPrice as (Quantity * UnitPrice.Amount) minus DiscountAmount.Amount using UnitPrice.Currency.
        /// </summary>
        private void RecalculateTotals()
        {
            TotalPrice = new((Quantity * UnitPrice.Amount) - DiscountAmount.Amount, UnitPrice.Currency);
        }
    }
}