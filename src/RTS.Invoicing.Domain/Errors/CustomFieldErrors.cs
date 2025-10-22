using RTS.Invoicing.Domain.Common;

namespace RTS.Invoicing.Domain.Errors
{
    public static class CustomFieldErrors
    {
        public static readonly Error NotFoundInMerchantCustomFields = new(
            "CustomField.NotFoundInMerchantCustomFields", "Custom Field provided is not found within this merchant custom fields.");

        public static readonly Error InvalidName = new(
            "CustomField.Name.Invalid", $"Custom field name must be between '{Constants.MIN_CUSTOM_FIELDS_CHARACTERS}' and '{Constants.MAX_CUSTOM_FIELDS_CHARACTERS}' characters.");

        public static readonly Error MerchantMismatch = new(
            "CustomField.MerchantMismatch", $"Merchant custom fields should be used for within an invoice of the same merchant.");

        public static readonly Error DuplicatedCustomField = new(
            "CustomField.DuplicatedCustomField", "The custom field provided already exists.");
    }
}
