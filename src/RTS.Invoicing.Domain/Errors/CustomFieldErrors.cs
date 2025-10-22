using RTS.Invoicing.Domain.Common;

namespace RTS.Invoicing.Domain.Errors
{
    /// <summary>
    /// Contains predefined <see cref="Error"/> constants related to custom field operations.
    /// </summary>
    public static class CustomFieldErrors
    {
        /// <summary>
        /// Represents an error indicating that a specified custom field
        /// does not exist in the merchant's defined list of custom fields.
        /// </summary>
        public static readonly Error NotFoundInMerchantCustomFields = new(
            "CustomField.NotFoundInMerchantCustomFields", "Custom Field provided is not found within this merchant custom fields.");

        /// <summary>
        /// Represents an error indicating that the custom field name's length
        /// is outside the allowed character limits.
        /// </summary>
        public static readonly Error InvalidName = new(
            "CustomField.Name.Invalid", $"Custom field name must be between '{Constants.MIN_CUSTOM_FIELDS_CHARACTERS}' and '{Constants.MAX_CUSTOM_FIELDS_CHARACTERS}' characters.");

        /// <summary>
        /// Represents an error indicating that a custom field is being applied
        /// to an entity (like an invoice) that belongs to a different merchant.
        /// </summary>
        public static readonly Error MerchantMismatch = new(
            "CustomField.MerchantMismatch", $"Merchant custom fields should be used for within an invoice of the same merchant.");

        /// <summary>
        /// Represents an error indicating that the custom field being added
        /// already exists, likely violating a uniqueness constraint.
        /// </summary>
        public static readonly Error DuplicatedCustomField = new(
            "CustomField.DuplicatedCustomField", "The custom field provided already exists.");
    }
}
