using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.Aggregates.Merchants.Enums;
using RTS.Invoicing.Domain.Errors;

namespace RTS.Invoicing.Domain.Aggregates.Merchants
{
    /// <summary>
    /// Represents a dynamic, user-defined custom field configuration within the system, scoped to a specific merchant.
    /// This entity allows for the definition of extra data fields without changing the core database schema.
    /// </summary>
    public class CustomField : AuditableEntity<CustomFieldId>
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="CustomField"/> class from being created.
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
        /// <param name="type">The type.</param>
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
        /// Creates a new <see cref="CustomField"/> instance for the specified merchant with the given name and type.
        /// </summary>
        /// <remarks>The method validates the provided <paramref name="name"/> to ensure it is not empty
        /// and meets the required length constraints. If the validation fails, a failure result is returned with the
        /// corresponding error.</remarks>
        /// <param name="merchantId">The unique identifier of the merchant for whom the custom field is being created.</param>
        /// <param name="name">The name of the custom field. Must not be null, empty, or consist only of whitespace, and must meet the
        /// required length constraints.</param>
        /// <param name="type">The type of the custom field, indicating its data format or purpose.</param>
        /// <returns>A <see cref="Result{T}"/> containing the created <see cref="CustomField"/> if the operation succeeds; 
        /// otherwise, a failure result with the appropriate error.</returns>
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
        /// </summary>
        /// <param name="newName">The new name to assign to the custom field.</param>
        /// <returns>
        /// A <see cref="Result"/> indicating success, or a failure <see cref="Result"/>
        /// containing an <see cref="Error"/> if the name is empty or has an invalid length.
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
        /// </returns>
        private static bool IsValidCustomFieldNameLength(string name)
        {
            return name.Length <= Constants.MAX_CUSTOM_FIELDS_CHARACTERS ||
                   name.Length >= Constants.MIN_CUSTOM_FIELDS_CHARACTERS;
        }
    }
}
