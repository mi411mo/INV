using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.Errors;

namespace RTS.Invoicing.Domain.Aggregates.Merchants
{
    /// <summary>
    /// Represents a master tax definition.
    /// Tax configurations are typically scoped to a specific merchant.
    /// </summary>
    public class Tax : Entity<TaxId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tax"/> class.
        /// </summary>
        private Tax()
            : base()
        {
            // For ORM Only.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tax"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="code">The code.</param>
        /// <param name="rate">The rate.</param>
        /// <param name="notes">The notes.</param>
        private Tax(
            TaxId id,
            MerchantId merchantId,
            string name,
            string code,
            decimal rate,
            string? notes = null)
            : base(id)
        {
            MerchantId = merchantId;
            Name = name;
            Code = code;
            Rate = rate;
            IsActive = true;
            Notes = notes;
        }

        /// <summary>
        /// Gets or sets the unique identifier of the merchant who defined this tax.
        /// </summary>
        public MerchantId MerchantId { private set; get; } = null!;

        /// <summary>
        /// Gets or sets the human-readable name of the tax (e.g., "Value Added Tax", "State Sales Tax").
        /// </summary>
        public string Name { private set; get; } = string.Empty;

        /// <summary>
        /// Gets or sets the short code or abbreviation for the tax (e.g., "VAT", "GST").
        /// </summary>
        public string Code { private set; get; } = string.Empty;

        /// <summary>
        /// Gets or sets the fractional rate of the tax (e.g., 0.15 for 15%).
        /// </summary>
        public decimal Rate { private set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether this tax definition is currently active and can be applied to new invoices.
        /// </summary>
        public bool IsActive { private set; get; } = true;

        public string? Notes { private set; get; } = null;

        /// <summary>
        /// Creates the specified merchant identifier.
        /// </summary>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="code">The code.</param>
        /// <param name="rate">The rate.</param>
        /// <param name="notes">The notes.</param>
        /// <returns></returns>
        public static Result<Tax> Create(
            MerchantId merchantId,
            string name,
            string code,
            decimal rate,
            string? notes = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result.Failure<Tax>(TaxErrors.NameRequired);
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                return Result.Failure<Tax>(TaxErrors.CodeRequired);
            }

            if (rate < 0 || rate > 100)
            {
                return Result.Failure<Tax>(TaxErrors.RateOutOfRange);
            }

            var tax = new Tax(new TaxId(0), merchantId, name, code, rate, notes);
            return Result.Success(tax);
        }

        /// <summary>
        /// Updates the core details of the tax definition.
        /// </summary>
        /// <param name="name">The new name for the tax.</param>
        /// <param name="code">The new code for the tax.</param>
        /// <param name="rate">The new percentage rate for the tax.</param>
        /// <returns>
        /// A <see cref="Result"/> indicating success, or a failure <see cref="Result"/>
        /// if the name or code is empty, or if the rate is out of range.
        /// </returns>
        public Result UpdateDetails(
            string name,
            string code,
            decimal rate)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result.Failure(TaxErrors.NameRequired);
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                return Result.Failure<Tax>(TaxErrors.CodeRequired);
            }

            if (rate < 0 || rate > 100)
            {
                return Result.Failure<Tax>(TaxErrors.RateOutOfRange);
            }

            Name = name;
            Code = code;
            Rate = rate;

            return Result.Success();
        }

        /// <summary>
        /// Deactivates this instance.
        /// </summary>
        public void Deactivate()
        {
            if (IsActive)
            {
                IsActive = false;
            }
        }

        /// <summary>
        /// Activates this instance.
        /// </summary>
        public void Activate()
        {
            if (!IsActive)
            {
                IsActive = true;
            }
        }

        /// <summary>
        /// Applies the current tax to the specified amount.
        /// </summary>
        /// <param name="amount">The amount to apply the tax on.</param>
        /// <returns>The new calculated amount.</returns>
        public decimal ApplyTax(decimal amount)
        {
            decimal result = amount;

            if (Rate > 0)
            {
                result *= Rate;
            }

            return result;
        }

        /// <summary>
        /// Updates the tax notes.
        /// </summary>
        /// <param name="notes">The notes.</param>
        public void UpdateNotes(string? notes)
        {
            Notes = notes;
        }
    }
}
