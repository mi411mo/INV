using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.Aggregates.Merchants;
using RTS.Invoicing.Domain.ValueObjects;

namespace RTS.Invoicing.Domain.Aggregates.Invoices
{
    /// <summary>
    /// Represents the link between an Invoice and a specific Tax, capturing the amount of tax applied.
    /// This entity often serves as a junction table in a many-to-many relationship.
    /// </summary>
    public class InvoiceItemTax : Entity<InvoiceItemTaxId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvoiceItemTax"/> entity.
        /// </summary>
        private InvoiceItemTax()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvoiceItemTax"/> class with the specified invoice item, tax
        /// details, and calculated tax values.
        /// </summary>
        /// <param name="invoiceItemId">The unique identifier of the invoice item to which the tax is applied.</param>
        /// <param name="taxId">The unique identifier of the tax being applied.</param>
        /// <param name="taxRateApplied">The tax rate, expressed as a decimal, that was applied to the invoice item.</param>
        /// <param name="taxAmount">The monetary value of the tax calculated for the invoice item.</param>
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

        /// <summary>
        /// Gets the associated invoice for the current entity.
        /// </summary>
        public virtual Invoice Invoice { private set; get; } = null!;

        /// <summary>
        /// Gets the tax information associated with the current entity.
        /// </summary>
        public virtual Tax Tax { private set; get; } = null!;

        /// <summary>
        /// Factory method to create an InvoiceItemTax instance. This is the only way to create this object.
        /// It centralizes the business logic for tax calculation.
        /// </summary>
        /// <param name="item">The parent invoice item.</param>
        /// <param name="tax">The tax to apply.</param>
        /// <param name="currency">The currency of the invoice.</param>
        /// <returns>A new, valid InvoiceItemTax object.</returns>
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
