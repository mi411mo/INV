using RTS.Invoicing.Domain.Common;

namespace RTS.Invoicing.Domain.Errors
{
    public static class GeneralErrors
    {
        public static Error Empty(string propertyName) => new(
            "General.EmptyValue", $"The property '{propertyName}' cannot be empty.");
    }
}
