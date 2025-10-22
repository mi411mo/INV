using RTS.Invoicing.Domain.Common;

namespace RTS.Invoicing.Domain.Errors
{
    /// <summary>
    /// Contains predefined <see cref="Error"/> constants related to tax operations.
    /// </summary>
    public static class TaxErrors
    {
        /// <summary>
        /// Represents an error indicating that a specified tax
        /// does not exist in the merchant's defined collection of taxes.
        /// </summary>
        public static readonly Error NotFoundInMerchantTaxes = new(
            "Tax.NotFoundInMerchantTaxes", "Tax provided is not found within this merchant taxes.");

        /// <summary>
        /// Represents an error indicating that the tax name is required and cannot be empty.
        /// </summary>
        public static readonly Error NameRequired = new(
            "Tax.Name.Required", "Tax name is required.");

        /// <summary>
        /// Represents an error indicating that the tax code is required and cannot be empty.
        /// </summary>
        public static readonly Error CodeRequired = new(
            "Tax.Code.Required", "Tax code is required.");

        /// <summary>
        /// Represents an error indicating that the tax rate must be a value between 0 and 100.
        /// </summary>
        public static readonly Error RateOutOfRange = new(
            "Tax.Rate.OutOfRange", "Tax rate must be between 0 and 100.");

        /// <summary>
        /// Represents an error indicating that the provided tax code
        /// already exists for the merchant, violating a uniqueness constraint.
        /// </summary>
        public static readonly Error DuplicatedCode = new(
            "Tax.Code.DuplicatedCode", "The code provided already exists.");
    }
}
