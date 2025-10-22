using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.Entities.Merchants;
using RTS.Invoicing.Domain.ValueObjects;

namespace RTS.Invoicing.Domain.Entities.Invoices
{
    /// <summary>
    /// Represents the link between an Invoice and a specific Tax, capturing the amount of tax applied.
    /// This entity often serves as a junction table in a many-to-many relationship.
    /// </summary>
    public class InvoiceItemTax : Entity<InvoiceItemTaxId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvoiceItemTax"/> entity.
        /// <summary>
        /// Initializes a new instance of the InvoiceItemTax class required by ORM/EF materializers.
        /// </summary>
        private InvoiceItemTax()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="InvoiceItemTax"/> with the given invoice item and tax identifiers, the tax rate that was applied, and the calculated tax amount.
        /// </summary>
        /// <param name="invoiceItemId">Identifier of the invoice item this tax is associated with.</param>
        /// <param name="taxId">Identifier of the tax applied to the invoice item.</param>
        /// <param name="taxRateApplied">The tax rate (percentage) that was applied when the invoice was created.</param>
        /// <param name="taxAmount">The monetary amount of tax calculated for the invoice item.</param>
        private InvoiceItemTax(
            InvoiceItemId invoiceItemId,
            TaxId taxId,
            decimal taxRateApplied,
            Money taxAmount)
        {
            InvoiceItemId = invoiceItemId;
            TaxId = taxId;
            TaxRateApplied = taxRateApplied;
            TaxAmount = taxAmount;
        }

        /// <summary>
        /// Gets or sets the foreign key referencing the parent Invoice entity.
        /// </summary>
        public InvoiceItemId InvoiceItemId { private set; get; } = null!;

        /// <summary>
        /// Gets or sets the foreign key referencing the <see cref="Tax"/> entity that was applied.
        /// </summary>
        public TaxId TaxId { private set; get; } = null!;

        /// <summary>
        /// Gets or sets the tax rate from the Taxes table at the time of invoice creation.
        /// </summary>
        public decimal TaxRateApplied { private set; get; }

        /// <summary>
        /// Gets or sets the calculated tax for this specific item line.
        /// </summary>
        public Money TaxAmount { private set; get; } = null!;

        // Navigation Properties
        public virtual Invoice Invoice { private set; get; } = null!;
        public virtual Tax Tax { private set; get; } = null!;

        /// <summary>
        /// Factory method to create an InvoiceItemTax instance. This is the only way to create this object.
        /// It centralizes the business logic for tax calculation.
        /// </summary>
        /// <param name="item">The parent invoice item.</param>
        /// <param name="tax">The tax to apply.</param>
        /// <param name="currency">The currency of the invoice.</param>
        /// <summary>
        /// Create an InvoiceItemTax for the specified invoice item and tax using the provided currency.
        /// </summary>
        /// <param name="item">The invoice item from which taxable amount and identifiers are taken.</param>
        /// <param name="tax">The tax whose rate will be applied to the invoice item.</param>
        /// <param name="currency">The currency code used to construct the tax amount (e.g., ISO currency code).</param>
        /// <returns>A Result containing the newly created InvoiceItemTax with the tax amount computed from the item's taxable amount (item total minus discount, clamped to zero) and the tax rate (stored as 15.00 for 15%).</returns>
        internal static Result<InvoiceItemTax> Create(InvoiceItem item, Tax tax, string currency)
        {
            var taxableAmount = item.TotalPrice.Amount - item.DiscountAmount.Amount;
            if (taxableAmount < 0)
            {
                taxableAmount = 0;
            }

            // Rate is stored as 15.00 for 15%, so divide by 100 for calculation.
            var taxAmountValue = taxableAmount * (tax.Rate / 100m);

            var taxAmount = new Money(taxAmountValue, Currency.Create(currency).Value!);

            var invoiceItemTax = new InvoiceItemTax(
                item.Id,
                tax.Id,
                tax.Rate,
                taxAmount);

            return Result.Success(invoiceItemTax);
        }
    }
}