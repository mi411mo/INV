using RTS.Invoicing.Domain.Common;

namespace RTS.Invoicing.Domain.Errors
{
    /// <summary>
    /// Contains predefined <see cref="Error"/> constants related to email validation.
    /// </summary>
    public static class EmailErrors
    {
        /// <summary>
        /// Represents an error indicating that an email address was not provided.
        /// </summary>
        public static readonly Error Empty = new("Email.Empty", "Email is required.");

        /// <summary>
        /// Represents an error indicating that the provided email address
        /// does not follow a valid format.
        /// </summary>
        public static readonly Error InvalidFormat = new("Email.InvalidFormat", "Email format is invalid.");
    }
}
