using RTS.Invoicing.Domain.Common;

namespace RTS.Invoicing.Domain.Errors
{
    public static class EmailErrors
    {
        public static readonly Error Empty = new("Email.Empty", "Email is required.");
        public static readonly Error InvalidFormat = new("Email.InvalidFormat", "Email format is invalid.");
    }
}
