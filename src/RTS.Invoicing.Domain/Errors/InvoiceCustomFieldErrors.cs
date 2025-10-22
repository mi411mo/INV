using RTS.Invoicing.Domain.Common;

namespace RTS.Invoicing.Domain.Errors
{
    public static class InvoiceCustomFieldErrors
    {
        public static readonly Error ValueTooLong = new(
            "InvoiceCustomField.Value.TooLong", $"The value for a custom field cannot exceed '{Constants.MAX_CUSTOM_FIELD_VALUE_CHARACTERS}' characters.");

        public static readonly Error InvalidValueType = new(
            "InvoiceCustomField.InvalidValueType", "Invalid value for this custom field.");
    }
}
