namespace RTS.Invoicing.Domain.Aggregates.Merchants.Enums
{
    /// <summary>
    /// Defines the data types available for custom fields.
    /// </summary>
    public enum CustomFieldsTypes : byte
    {
        /// <summary>
        /// Represents a numeric value.
        /// </summary>
        Number = 1,

        /// <summary>
        /// Represents a text or string value.
        /// </summary>
        Text = 2,

        /// <summary>
        /// Represents a date value (without time).
        /// </summary>
        Date = 3,

        /// <summary>
        /// Represents a date and time value.
        /// </summary>
        DateTime = 4,

        /// <summary>
        /// Represents a boolean value (true/false).
        /// </summary>
        Boolean = 5
    }
}
