using RTS.Invoicing.Domain.Common;

namespace RTS.Invoicing.Domain.Errors
{
    public static class PersonErrors
    {
        public static readonly Error InvalidName = new(
            "Person.InvalidName", "Person name should be a valid name.");

        public static readonly Error InvalidEmailFormat = new(
            "Person.Email.InvalidFormat", "The email address has an invalid format.");
    }
}
