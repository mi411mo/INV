using RTS.Invoicing.Domain.Common;

namespace RTS.Invoicing.Domain.Errors
{
    public static class TaxErrors
    {
        public static readonly Error NotFoundInMerchantTaxes = new(
            "Tax.NotFoundInMerchantTaxes", "Tax provided is not found within this merchant taxes.");

        public static readonly Error NameRequired = new(
            "Tax.Name.Required", "Tax name is required.");

        public static readonly Error CodeRequired = new(
            "Tax.Code.Required", "Tax code is required.");

        public static readonly Error RateOutOfRange = new(
            "Tax.Rate.OutOfRange", "Tax rate must be between 0 and 100.");

        public static readonly Error DuplicatedCode = new(
            "Tax.Code.DuplicatedCode", "The code provided already exists.");
    }
}
