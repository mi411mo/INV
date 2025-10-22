using RTS.Invoicing.Domain.Common;

namespace RTS.Invoicing.Domain.Errors
{
    /// <summary>
    /// Contains predefined <see cref="Error"/> constants related to invoice line items.
    /// </summary>
    public static class InvoiceItemErrors
    {
        /// <summary>
        /// Represents an error indicating that the item's quantity or unit price is invalid.
        /// </summary>
        public static readonly Error InvalidAmount = new(
            "InvoiceItem.InvalidAmount", "Quantity must be positive and unit price non-negative.");

        /// <summary>
        /// Represents an error indicating that the invoice item is not associated with a valid invoice.
        /// </summary>
        public static readonly Error InvalidInvoiceId = new(
            "InvoiceItem.InvalidInvoiceId", "Invoice items should be linked with a valid invoice.");

        /// <summary>
        /// Represents an error indicating that the item's description is missing or invalid.
        /// </summary>
        public static readonly Error InvalidDescription = new(
            "InvoiceItem.InvalidDescription", "Invoice items should contain a valid description.");

        /// <summary>
        /// Represents an error indicating that the item's quantity must be a positive number.
        /// </summary>
        public static readonly Error InvalidQuantity = new(
            "InvoiceItem.InvalidQuantity", "Invoice items quantity should be a positive value.");

        /// <sSummary>
        /// Represents an error indicating that the item's unit price cannot be a negative number.
        /// </sSummary>
        public static readonly Error InvalidUnitPrice = new(
            "InvoiceItem.InvalidUnitPrice", "Unit price cannot be negative.");

        /// <summary>
        /// Represents an error indicating that the applied discount is greater than the item's total price.
        /// </summary>
        public static readonly Error DiscountExceedsTotal = new(
            "InvoiceItem.DiscountExceedsTotal", "Invoice item discount cannot exceed the total price of the item.");

        /// <summary>
        /// Represents an error indicating that the discount amount cannot be a negative number.
        /// </summary>
        public static readonly Error InvalidDiscount = new(
            "InvoiceLine.InvalidDiscount", "Discount amount cannot be negative.");

        /// <summary>
        /// Represents an error indicating that a specific tax has already been added to this line item.
        /// </summary>
        public static readonly Error DuplicatedTax = new(
            "InvoiceLine.DuplicatedTax", "This tax has already been applied to the item.");
    }
}
