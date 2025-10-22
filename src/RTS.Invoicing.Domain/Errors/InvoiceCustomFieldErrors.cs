using RTS.Invoicing.Domain.Common;

namespace RTS.Invoicing.Domain.Errors
{
    /// <summary>
    /// Contains predefined <see cref="Error"/> constants related to invoice custom fields.
    /// </summary>
    public static class InvoiceCustomFieldErrors
    {
        /// <summary>
        /// Represents an error indicating that the value provided for a custom field
        /// exceeds the maximum allowed character limit.
        /// </summary>
        public static readonly Error ValueTooLong = new(
            "InvoiceCustomField.Value.TooLong", $"The value for a custom field cannot exceed '{Constants.MAX_CUSTOM_FIELD_VALUE_CHARACTERS}' characters.");

        /// <summary>
        /// Represents an error indicating that the value provided for a custom field
        /// does not match the expected data type (e.g., providing text for a number field).
        /// </summary>
        public static readonly Error InvalidValueType = new(
            "InvoiceCustomField.InvalidValueType", "Invalid value for this custom field.");
    }
}
