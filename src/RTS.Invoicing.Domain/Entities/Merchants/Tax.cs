using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.Errors;

namespace RTS.Invoicing.Domain.Entities.Merchants
{
    /// <summary>
    /// Represents a master tax definition.
    /// Tax configurations are typically scoped to a specific merchant.
    /// </summary>
    public class Tax : Entity<TaxId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tax"/> class.
        /// <summary>
        /// Initializes a new instance of <see cref="Tax"/> for ORM materialization.
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
        /// <summary>
        /// Initializes a new Tax instance with the specified identity and property values; the tax is created as active.
        /// </summary>
        /// <param name="id">The tax identifier.</param>
        /// <param name="merchantId">Identifier of the merchant that defines the tax.</param>
        /// <param name="name">Human-readable name of the tax.</param>
        /// <param name="code">Short code or abbreviation for the tax.</param>
        /// <param name="rate">Tax rate as a decimal (0–100 representing percentage).</param>
        /// <param name="notes">Optional notes about the tax.</param>
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
        /// <summary>
        /// Creates a new Tax for the specified merchant after validating the provided fields.
        /// </summary>
        /// <param name="merchantId">Identifier of the merchant that owns the tax.</param>
        /// <param name="name">Human-readable name of the tax; must not be null, empty, or whitespace.</param>
        /// <param name="code">Short code or abbreviation for the tax; must not be null, empty, or whitespace.</param>
        /// <param name="rate">Tax rate as a percentage; must be between 0 and 100 inclusive.</param>
        /// <param name="notes">Optional notes about the tax.</param>
        /// <returns>
        /// A Result&lt;Tax&gt; containing the created Tax on success; on failure contains a validation error:
        /// `TaxErrors.NameRequired`, `TaxErrors.CodeRequired`, or `TaxErrors.RateOutOfRange`.
        /// </returns>
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
        /// Updates the tax's name, code, and rate after validating the provided values.
        /// </summary>
        /// <param name="name">The new human-readable tax name.</param>
        /// <param name="code">The new short tax code or abbreviation.</param>
        /// <param name="rate">The new tax rate as a percentage (0 to 100).</param>
        /// <returns>
        /// A <see cref="Result"/> indicating success, or failure containing one of the validation errors:
        /// <see cref="TaxErrors.NameRequired"/>, <see cref="TaxErrors.CodeRequired"/>, or <see cref="TaxErrors.RateOutOfRange"/>.
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
        /// <summary>
        /// Marks the tax as inactive so it will no longer be applied.
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
        /// <summary>
        /// Marks the tax as active if it is currently inactive.
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
        /// <summary>
        — Calculates the amount after applying this tax by multiplying the provided base amount by the tax <c>Rate</c> when <c>Rate</c> is greater than zero.
        /// </summary>
        /// <param name="amount">The base monetary amount to which the tax rate will be applied.</param>
        /// <returns>The amount after applying the tax rate; if <c>Rate</c> is zero or less, returns the original <paramref name="amount"/>.</returns>
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
        /// <summary>
        /// Sets or clears the tax's optional notes.
        /// </summary>
        /// <param name="notes">The notes text to associate with the tax, or null to clear existing notes.</param>
        public void UpdateNotes(string? notes)
        {
            Notes = notes;
        }
    }
}