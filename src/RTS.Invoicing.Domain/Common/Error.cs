namespace RTS.Invoicing.Domain.Common
{
    /// <summary>
    /// Represents a specific error with a code and description.
    /// </summary>
    /// <param name="Code">The unique error code.</param>
    /// <param name="Description">The descriptive message for the error.</param>
    public sealed record Error(string Code, string Description)
    {
        /// <summary>
        /// Gets an <see cref="Error"/> instance representing no error.
        /// </summary>
        /// <remarks>
        /// This is often used to indicate a successful operation.
        /// </remarks>
        public static readonly Error None = new(string.Empty, string.Empty);

        /// <summary>
        /// Gets an <see cref="Error"/> instance representing a null value error.
        /// </summary>
        /// <remarks>
        /// This is used when an operation unexpectedly results in a null value.
        /// </remarks>
        public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.");
    }
}
