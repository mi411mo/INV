using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.Enums;
using RTS.Invoicing.Domain.Errors;

namespace RTS.Invoicing.Domain.Entities.Merchants
{
    /// <summary>
    /// Represents a dynamic, user-defined custom field configuration within the system, scoped to a specific merchant.
    /// This entity allows for the definition of extra data fields without changing the core database schema.
    /// </summary>
    public class CustomField : AuditableEntity<CustomFieldId>
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="CustomField"/> class from being created.
        /// <summary>
        /// Initializes a new instance of CustomField for ORM materialization only.
        /// </summary>
        private CustomField()
            : base()
        {
            // For ORM Only.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomField"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="name">The name.</param>
        /// <summary>
        /// Initializes a new CustomField with the specified identifier, merchant, name, and field type.
        /// </summary>
        /// <param name="id">The unique identifier for the custom field.</param>
        /// <param name="merchantId">The identifier of the merchant that owns the custom field.</param>
        /// <param name="name">The display name of the custom field.</param>
        /// <param name="type">The data type of the custom field.</param>
        private CustomField(
            CustomFieldId id,
            MerchantId merchantId,
            string name,
            CustomFieldsTypes type)
            : base(id)
        {
            this.MerchantId = merchantId;
            this.Name = name;
            this.Type = type;
        }

        /// <summary>
        /// Gets or sets the unique identifier of the merchant who issued the invoice.
        /// </summary>
        public MerchantId MerchantId { private set; get; } = null!;

        /// <summary>
        /// Gets or sets the name or label of the custom field as it should be displayed.
        /// </summary>
        public string Name { private set; get; } = string.Empty;

        /// <summary>
        /// Gets or sets the data type of the custom field (e.g., text, number, date).
        /// </summary>
        public CustomFieldsTypes Type { private set; get; }

        /// <summary>
        /// Create a validated custom field configured for the specified merchant.
        /// </summary>
        /// <param name="merchantId">The merchant identifier that will own the custom field.</param>
        /// <param name="name">The display name for the custom field; must not be empty or whitespace and must satisfy length constraints.</param>
        /// <param name="type">The data type of the custom field.</param>
        /// <returns>A <see cref="Result{CustomField}"/> containing the created CustomField when validation succeeds, or a failed Result with the validation error when it fails.</returns>
        public static Result<CustomField> Create(
            MerchantId merchantId,
            string name,
            CustomFieldsTypes type)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result.Failure<CustomField>(GeneralErrors.Empty("Custom Field Name"));
            }

            if (!IsValidCustomFieldNameLength(name))
            {
                return Result.Failure<CustomField>(CustomFieldErrors.InvalidName);
            }

            var customField = new CustomField(
                new CustomFieldId(0),
                merchantId,
                name,
                type);
            return Result.Success(customField);
        }

        /// <summary>
        /// Updates the name of the custom field.
        /// <summary>
        /// Updates the custom field's display name after validating the provided value.
        /// </summary>
        /// <param name="newName">The new display label for the custom field.</param>
        /// <returns>
        /// A Result indicating success when the name was set; a failure with
        /// GeneralErrors.Empty("Custom Field Name") if <paramref name="newName"/> is null, empty, or whitespace,
        /// or a failure with CustomFieldErrors.InvalidName if the name's length is outside allowed bounds.
        /// </returns>
        public Result UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                return Result.Failure(GeneralErrors.Empty("Custom Field Name"));
            }

            if (!IsValidCustomFieldNameLength(newName))
            {
                return Result.Failure(CustomFieldErrors.InvalidName);
            }

            Name = newName;
            return Result.Success();
        }

        /// <summary>
        /// Helper method to determines whether the specified name is valid custom field name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        ///   <c>true</c> if [is valid custom field name] [the specified name]; otherwise, <c>false</c>.
        /// <summary>
        /// Determines whether the provided custom field name's length is less than or equal to the configured maximum or greater than or equal to the configured minimum.
        /// </summary>
        /// <param name="name">The candidate custom field name to validate.</param>
        /// <returns>`true` if the name length is less than or equal to <c>Constants.MAX_CUSTOM_FIELDS_CHARACTERS</c> or greater than or equal to <c>Constants.MIN_CUSTOM_FIELDS_CHARACTERS</c>, `false` otherwise.</returns>
        private static bool IsValidCustomFieldNameLength(string name)
        {
            return name.Length <= Constants.MAX_CUSTOM_FIELDS_CHARACTERS ||
                   name.Length >= Constants.MIN_CUSTOM_FIELDS_CHARACTERS;
        }
    }
}