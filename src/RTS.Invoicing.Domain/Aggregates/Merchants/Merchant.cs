using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.Aggregates.Merchants.Enums;
using RTS.Invoicing.Domain.Errors;
using RTS.Invoicing.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RTS.Invoicing.Domain.Aggregates.Merchants
{
    /// <summary>
    /// Represents the Merchant aggregate root.
    /// </summary>
    public class Merchant : AggregateRoot<MerchantId>
    {
        private readonly List<Tax> _taxes = new();
        private readonly List<CustomField> _customFields = new();
        private readonly List<InvoiceSequence> _invoiceSequences = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="Merchant"/> class.
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
        /// <param name="defaultCurrency">The default currency code.</param>
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

        /// <summary>
        /// Gets the list of taxes defined by the merchant.
        /// </summary>
        public IReadOnlyList<Tax> Taxes => _taxes.AsReadOnly();

        /// <summary>
        /// Gets the list of custom field definitions for the merchant.
        /// </summary>
        public IReadOnlyList<CustomField> CustomFields => _customFields.AsReadOnly();

        /// <summary>
        /// Creates the specified merchant.
        /// </summary>
        /// <param name="arabicName">Merchant Arabic name.</param>
        /// <param name="englishName">Merchant English name.</param>
        /// <param name="defaultCurrencyCode">The default currency code.</param>
        /// <returns></returns>
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
        /// <param name="settingsAsJson">A string, typically JSON, representing the configuration.</param>
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
        /// <returns></returns>
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
        /// Add a new merchant tax.
        /// </summary>
        /// <param name="name">The tax name.</param>
        /// <param name="code">The tax code.</param>
        /// <param name="rate">The tax rate.</param>
        /// <param name="note">The note.</param>
        /// <returns>A <see cref="Result"/> indicating success or failure.</returns>
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
        /// <returns></returns>
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
        /// Updates the name of an existing custom field definition.
        /// </summary>
        /// <param name="customFieldId">The ID of the custom field to update.</param>
        /// <param name="newName">The new name to assign to the custom field.</param>
        /// <returns>
        /// A <see cref="Result"/> indicating success, or failure if the field is not found,
        /// the name is invalid, or the name is a duplicate.
        /// </returns>
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
        /// Generates the next invoice number for a given prefix.
        /// </summary>
        /// <remarks>
        /// Creates a new sequence if one for the prefix does not exist.
        /// </remarks>
        /// <param name="prefix">The prefix for the invoice number (e.g., "INV-").</param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing the formatted next invoice number (e.g., "INV-000001")
        /// or an <see cref="Error"/> if the prefix is invalid.
        /// </returns>
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
        /// </summary>
        public void Activate()
        {
            IsActive = true;
        }

        /// <summary>
        /// Deactivates this merchant.
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
        /// <returns>A <see cref="Result"/> indicating success or failure.</returns>
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
