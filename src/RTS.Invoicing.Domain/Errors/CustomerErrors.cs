using RTS.Invoicing.Domain.Common;

namespace RTS.Invoicing.Domain.Errors
{
    /// <summary>
    /// Contains predefined <see cref="Error"/> constants related to customer operations.
    /// </summary>
    public static class CustomerErrors
    {
        /// <summary>
        /// Represents an error indicating that a valid person identifier is required
        /// when creating a customer.
        /// </summary>
        public static readonly Error InvalidPersonId = new(
            "Customer.PersonId.Invalid", "A valid person is required to create a customer.");

        /// <summary>
        /// Represents an error indicating that the customer's reference identifier
        /// cannot be null or empty.
        /// </summary>
        public static readonly Error EmptyReference = new(
            "Customer.EmptyReference", "Customer reference cannot be empty.");

        /// <summary>
        /// Represents an error indicating that an operation cannot be completed
        /// because the customer has been marked as deleted.
        /// </summary>
        public static readonly Error CustomerIsDeleted = new(
            "Customer.IsDeleted", "This operation cannot be performed on a deleted customer.");
    }
}
