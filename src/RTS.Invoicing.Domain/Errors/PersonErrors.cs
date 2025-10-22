using RTS.Invoicing.Domain.Common;

namespace RTS.Invoicing.Domain.Errors
{
    /// <summary>
    /// Contains predefined <see cref="Error"/> constants related to person data.
    /// </summary>
    public static class PersonErrors
    {
        /// <summary>
        /// Represents an error indicating that the person's name is invalid or empty.
        /// </summary>
        public static readonly Error InvalidName = new(
            "Person.InvalidName", "Person name should be a valid name.");

        /// <summary>
        /// Represents an error indicating that the person's email address
        /// does not follow a valid format.
        /// </summary>
        public static readonly Error InvalidEmailFormat = new(
            "Person.Email.InvalidFormat", "The email address has an invalid format.");
    }
}
