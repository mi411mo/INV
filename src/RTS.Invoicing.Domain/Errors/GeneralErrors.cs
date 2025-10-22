using RTS.Invoicing.Domain.Common;

namespace RTS.Invoicing.Domain.Errors
{
    public static class GeneralErrors
    {
        /// <summary>
            /// Creates an error indicating the specified property cannot be empty.
            /// </summary>
            /// <param name="propertyName">The name of the property that must not be empty.</param>
            /// <returns>An <see cref="Error"/> with code "General.EmptyValue" and a message stating the property cannot be empty.</returns>
            public static Error Empty(string propertyName) => new(
            "General.EmptyValue", $"The property '{propertyName}' cannot be empty.");
    }
}