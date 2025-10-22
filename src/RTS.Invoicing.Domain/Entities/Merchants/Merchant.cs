using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.Enums;
using RTS.Invoicing.Domain.Errors;
using RTS.Invoicing.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RTS.Invoicing.Domain.Entities.Merchants
{
    public class Merchant : AggregateRoot<MerchantId>
    {
        private readonly List<Tax> _taxes = new();
        private readonly List<CustomField> _customFields = new();
        private readonly List<InvoiceSequence> _invoiceSequences = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="Merchant"/> class.
        /// <summary>
        /// Parameterless constructor used only by ORMs and serializers to materialize the entity.
        /// </summary>
        private Merchant()
            : base()
        {
            // for ORM only.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Merchant"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="arabicName">Merchant Arabic name.</param>
        /// <param name="englishName">Merchant English name.</param>
        /// <summary>
        /// Initializes a new Merchant with the specified identity, Arabic and English names, and default currency.
        /// </summary>
        /// <param name="id">The unique identifier of the merchant.</param>
        /// <param name="arabicName">The merchant's Arabic display name.</param>
        /// <param name="englishName">The merchant's English display name.</param>
        /// <param name="defaultCurrency">The merchant's default currency.</param>
        public Merchant(
            MerchantId id,
            string arabicName,
            string englishName,
            Currency defaultCurrency)
            : base(id)
        {
            ArabicName = arabicName;
            EnglishName = englishName;
            DefaultCurrency = defaultCurrency;
        }

        /// <summary>
        /// Gets or sets the person's name in Arabic.
        /// </summary>
        public string ArabicName { private set; get; } = string.Empty;

        /// <summary>
        /// Gets or sets the person's name in English.
        /// </summary>
        public string EnglishName { private set; get; } = string.Empty;

        /// <summary>
        /// Gets or sets the default currency code of the merchant.
        /// </summary>
        /// <value>The merchant default currency code. </value>
        public Currency DefaultCurrency { private set; get; } = null!;

        /// <summary>
        /// Gets or sets the merchant custom configuration settings.
        /// </summary>
        /// <value>The configuration settings.</value>
        public string? ConfigurationSettings { private set; get; } = null;

        /// <summary>
        /// Gets or sets a value indicating whether this merchant is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this merchant is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { private set; get; } = true;

        public IReadOnlyList<Tax> Taxes => _taxes.AsReadOnly();

        public IReadOnlyList<CustomField> CustomFields => _customFields.AsReadOnly();

        /// <summary>
        /// Creates the specified merchant.
        /// </summary>
        /// <param name="arabicName">Merchant Arabic name.</param>
        /// <param name="englishName">Merchant English name.</param>
        /// <param name="defaultCurrencyCode">The default currency code.</param>
        /// <summary>
        /// Creates a new Merchant with the given Arabic and English names and the specified default currency.
        /// </summary>
        /// <param name="arabicName">The merchant's Arabic name.</param>
        /// <param name="englishName">The merchant's English name; must not be null, empty, or whitespace.</param>
        /// <param name="defaultCurrencyCode">The ISO currency code to use as the merchant's default currency.</param>
        /// <returns>
        /// A Result containing the created Merchant on success; a failure Result with error code "Merchant.Name.Empty" if <paramref name="englishName"/> is null/empty/whitespace,
        /// or the underlying currency creation error if <paramref name="defaultCurrencyCode"/> is invalid.
        /// </returns>
        public static Result<Merchant> Create(string arabicName, string englishName, string defaultCurrencyCode)
        {
            if (string.IsNullOrWhiteSpace(englishName))
            {
                return Result.Failure<Merchant>(new Error("Merchant.Name.Empty", "English name cannot be empty."));
            }

            var currencyResult = Currency.Create(defaultCurrencyCode);
            if (currencyResult.IsFailure)
            {
                return Result.Failure<Merchant>(currencyResult.Error);
            }

            return new Merchant(new MerchantId(0), arabicName, englishName, currencyResult.Value!);
        }

        /// <summary>
        /// Updates the configuration settings for the merchant.
        /// </summary>
        /// <summary>
        /// Updates the merchant's configuration settings when provided with a JSON object or array string.
        /// </summary>
        /// <param name="settingsAsJson">Configuration as a JSON string; ignored if null, empty, or whitespace. The value is assigned only if it appears to be a JSON object (starts with '{' and ends with '}') or a JSON array (starts with '[' and ends with ']').</param>
        public void UpdateConfigurationSettings(string settingsAsJson)
        {
            if (string.IsNullOrWhiteSpace(settingsAsJson))
            {
                return;
            }

            // Validate JSON string
            if (
                (settingsAsJson.StartsWith('[') && settingsAsJson.EndsWith(']'))
                ||
                (settingsAsJson.StartsWith('{') && settingsAsJson.EndsWith('}'))
                )
            {
                ConfigurationSettings = settingsAsJson;
            }
        }

        /// <summary>
        /// Changes the merchant default currency.
        /// </summary>
        /// <param name="newDefaultCurrencyCode">The new default currency code.</param>
        /// <summary>
        /// Change the merchant's default currency to the currency identified by the provided currency code.
        /// </summary>
        /// <param name="newDefaultCurrencyCode">The ISO code of the new default currency.</param>
        /// <returns>`Success` result if the currency was updated or the code matches the current currency; `Failure` result containing the currency creation error if the provided code is invalid.</returns>
        public Result ChangeDefaultCurrency(string newDefaultCurrencyCode)
        {
            if (newDefaultCurrencyCode.Equals(DefaultCurrency.Code, StringComparison.OrdinalIgnoreCase))
            {
                return Result.Success();
            }

            var currencyResult = Currency.Create(newDefaultCurrencyCode);
            if (currencyResult.IsFailure)
            {
                return Result.Failure(currencyResult.Error);
            }

            DefaultCurrency = currencyResult.Value!;

            return Result.Success();
        }

        /// <summary>
        /// Add a new merchant  tax.
        /// </summary>
        /// <param name="name">The tax name.</param>
        /// <param name="code">The tax code.</param>
        /// <param name="rate">The tax rate.</param>
        /// <param name="note">The note.</param>
        /// <summary>
        /// Adds a tax to the merchant after validating that the tax code is unique and the rate is within allowed bounds.
        /// </summary>
        /// <param name="name">The display name of the tax.</param>
        /// <param name="code">The tax code; comparison is case-insensitive for duplication checks.</param>
        /// <param name="rate">The tax rate as a percentage; must be between 0 and 100 inclusive.</param>
        /// <param name="note">An optional note or description for the tax.</param>
        /// <returns>`Result` indicating success, or failure with `TaxErrors.DuplicatedCode`, `TaxErrors.RateOutOfRange`, or the error returned by `Tax.Create`.</returns>
        public Result AddTax(
            string name,
            string code,
            decimal rate,
            string? note = null)
        {
            if (_taxes.Any(t => t.Code.Equals(code, StringComparison.OrdinalIgnoreCase)))
            {
                return Result.Failure(TaxErrors.DuplicatedCode);
            }

            if (rate < 0 || rate > 100)
            {
                return Result.Failure(TaxErrors.RateOutOfRange);
            }

            var taxResult = Tax.Create(Id, name, code, rate, note);
            if (taxResult.IsFailure)
            {
                return taxResult;
            }

            _taxes.Add(taxResult.Value!);
            return Result.Success();
        }

        /// <summary>
        /// Adds a merchant-specific custom field.
        /// </summary>
        /// <param name="name">The custom field name.</param>
        /// <param name="type">The custom field data type.</param>
        /// <summary>
        /// Adds a new custom field to the merchant when the provided name is not already used.
        /// </summary>
        /// <param name="name">The display name of the custom field.</param>
        /// <param name="type">The custom field's data type.</param>
        /// <returns>`Result` indicating success, `Failure` with `CustomFieldErrors.DuplicatedCustomField` if a field with the same name exists, or `Failure` with another domain error describing why creation of the custom field failed.</returns>
        public Result AddCustomField(string name, CustomFieldsTypes type)
        {
            if (_customFields.Any(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                return Result.Failure(CustomFieldErrors.DuplicatedCustomField);
            }

            var customFieldResult = CustomField.Create(Id, name, type);
            if (customFieldResult.IsFailure)
            {
                return customFieldResult;
            }

            _customFields.Add(customFieldResult.Value!);
            return Result.Success();
        }

        /// <summary>
        /// Updates the name of a merchant's custom field identified by the given id.
        /// </summary>
        /// <param name="customFieldId">Identifier of the custom field to update.</param>
        /// <param name="newName">New name to assign to the custom field.</param>
        /// <returns>`Result` indicating success, or failure with one of: `CustomFieldErrors.NotFoundInMerchantCustomFields`, `CustomFieldErrors.InvalidName`, `CustomFieldErrors.DuplicatedCustomField`, or the error produced by the custom field update.</returns>
        public Result UpdateCustomField(CustomFieldId customFieldId, string newName)
        {
            var customField = _customFields.FirstOrDefault(t => t.Id == customFieldId);
            if (customField is null)
            {
                return Result.Failure(CustomFieldErrors.NotFoundInMerchantCustomFields);
            }

            if (string.IsNullOrWhiteSpace(newName))
            {
                return Result.Failure(CustomFieldErrors.InvalidName);
            }

            if (_customFields.Any(t => t.Name.Equals(newName, StringComparison.OrdinalIgnoreCase)))
            {
                return Result.Failure(CustomFieldErrors.DuplicatedCustomField);
            }

            var customFieldResult = customField.UpdateName(newName);
            if (customFieldResult.IsFailure)
            {
                return customFieldResult;
            }

            return Result.Success();
        }

        /// <summary>
        /// Generates the next invoice number.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <summary>
        /// Generate the next invoice number for the specified prefix, creating a sequence if one does not exist.
        /// </summary>
        /// <param name="prefix">The invoice prefix to use (case-insensitive).</param>
        /// <returns>The invoice identifier formatted as the prefix followed by a 6-digit zero-padded sequence (for example, "INV-000001"). Returns a failure Result if creating or retrieving the sequence fails.</returns>
        public Result<string> GenerateNextInvoiceNumber(string prefix)
        {
            var sequence = _invoiceSequences
                .FirstOrDefault(s => s.Prefix.Equals(prefix, StringComparison.OrdinalIgnoreCase));

            if (sequence is null)
            {
                var sequenceResult = InvoiceSequence.Create(Id, prefix);
                if (sequenceResult.IsFailure)
                {
                    return Result.Failure<string>(sequenceResult.Error);
                }

                sequence = sequenceResult.Value!;
                _invoiceSequences.Add(sequence);
            }

            int nextValue = sequence.GetNextSequence();

            return $"{prefix}{nextValue:D6}"; // "INV-000001"
        }

        /// <summary>
        /// Activates this merchant.
        /// <summary>
        /// Marks the merchant as active.
        /// </summary>
        public void Activate()
        {
            IsActive = true;
        }

        /// <summary>
        /// Deactivates this merchant.
        /// <summary>
        /// Marks the merchant as inactive by setting its IsActive flag to false.
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
        }

        /// <summary>
        /// Updates the tax details.
        /// </summary>
        /// <param name="taxId">The tax identifier to be updated.</param>
        /// <param name="newName">The tax new name.</param>
        /// <param name="newCode">The tax new code.</param>
        /// <param name="newRate">The tax new rate.</param>
        /// <param name="note">The note.</param>
        /// <summary>
        /// Updates an existing tax's name, code, rate and optionally its notes for this merchant.
        /// </summary>
        /// <param name="taxId">Identifier of the tax to update.</param>
        /// <param name="newName">The new name for the tax.</param>
        /// <param name="newCode">The new code for the tax.</param>
        /// <param name="newRate">The new tax rate; must be between 0 and 100 inclusive.</param>
        /// <param name="note">Optional notes to set on the tax; if null, existing notes are preserved.</param>
        /// <returns>`Success` if the tax was updated; a failure `Result` with a domain error when the tax is not found, the code is duplicated, the rate is out of range, or the underlying tax update fails.</returns>
        public Result UpdateTaxDetails(
            TaxId taxId,
            string newName,
            string newCode,
            decimal newRate,
            string? note = null)
        {
            var tax = _taxes.FirstOrDefault(t => t.Id == taxId);
            if (tax is null)
            {
                return Result.Failure(TaxErrors.NotFoundInMerchantTaxes);
            }

            if (_taxes.Any(t => t.Code.Equals(newCode, StringComparison.OrdinalIgnoreCase)))
            {
                return Result.Failure(TaxErrors.DuplicatedCode);
            }

            if (newRate < 0 || newRate > 100)
            {
                return Result.Failure(TaxErrors.RateOutOfRange);
            }

            var taxResult = tax.UpdateDetails(newName, newCode, newRate);
            if (taxResult.IsFailure)
            {
                return taxResult;
            }

            if (note is not null)
            {
                tax.UpdateNotes(note);
            }

            return Result.Success();
        }
    }
}