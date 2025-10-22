using RTS.Invoicing.Domain.Common;

namespace RTS.Invoicing.Domain.Errors
{

    public static class CustomerErrors
    {
        public static readonly Error InvalidPersonId = new(
            "Customer.PersonId.Invalid", "A valid person is required to create a customer.");

        public static readonly Error EmptyReference = new(
            "Customer.EmptyReference", "Customer reference cannot be empty.");

        public static readonly Error CustomerIsDeleted = new(
            "Customer.IsDeleted", "This operation cannot be performed on a deleted customer.");
    }

}
