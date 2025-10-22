using RTS.Invoicing.Domain.Common;

namespace RTS.Invoicing.Domain.Errors
{
    /// <summary>
    /// Contains methods for generating common, general-purpose errors.
    /// </summary>
    public static class GeneralErrors
    {
        /// <summary>
        /// Creates an error indicating that a specified property cannot be null or empty.
        /// </summary>
        /// <param name="propertyName">The name of the property that is empty.</param>
        /// <returns>A new <see cref="Error"/> instance describing the empty property.</returns>
        public static Error Empty(string propertyName) => new(
            "General.EmptyValue", $"The property '{propertyName}' cannot be empty.");
    }
}
