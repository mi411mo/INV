using RTS.Invoicing.Domain.Common;

namespace RTS.Invoicing.Domain.Errors
{
    public static class InvoiceItemErrors
    {
        public static readonly Error InvalidAmount = new(
            "InvoiceItem.InvalidAmount", "Quantity must be positive and unit price non-negative.");

        public static readonly Error InvalidInvoiceId = new(
            "InvoiceItem.InvalidInvoiceId", "Invoice items should be linked with a valid invoice.");

        public static readonly Error InvalidDescription = new(
            "InvoiceItem.InvalidDescription", "Invoice items should contain a valid description.");

        public static readonly Error InvalidQuantity = new(
            "InvoiceItem.InvalidQuantity", "Invoice items quantity should be a positive value.");

        public static readonly Error InvalidUnitPrice = new(
            "InvoiceItem.InvalidUnitPrice", "Unit price cannot be negative.");

        public static readonly Error DiscountExceedsTotal = new(
            "InvoiceItem.DiscountExceedsTotal", "Invoice item discount cannot exceed the total price of the item.");

        public static readonly Error InvalidDiscount = new(
            "InvoiceLine.InvalidDiscount", "Discount amount cannot be negative.");

        public static readonly Error DuplicatedTax = new(
            "InvoiceLine.DuplicatedTax", "This tax has already been applied to the item.");


    }
}
