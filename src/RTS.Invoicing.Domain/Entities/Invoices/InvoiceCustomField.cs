using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.Entities.Merchants;
using RTS.Invoicing.Domain.Errors;

namespace RTS.Invoicing.Domain.Entities.Invoices
{
    /// <summary>
    /// Represents the specific value assigned to a custom field for a particular invoice.
    /// This entity links an Invoice to a CustomField definition and stores the actual data value.
    /// </summary>
    public class InvoiceCustomField : Entity<InvoiceCustomFieldId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvoiceCustomField"/> class.
        /// <summary>
        /// Parameterless constructor used only by the ORM for object materialization.
        /// </summary>
        private InvoiceCustomField()
            : base()
        {
            // For ORM Only.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvoiceCustomField"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <param name="customFieldId">The custom field identifier.</param>
        /// <summary>
        /// Initializes a new instance of InvoiceCustomField with the specified identifiers and stored value for internal/ORM construction.
        /// </summary>
        /// <param name="id">The entity identifier for the invoice custom field.</param>
        /// <param name="invoiceId">The identifier of the parent invoice.</param>
        /// <param name="customFieldId">The identifier of the custom field definition.</param>
        /// <param name="value">The stored string value for the custom field.</param>
        private InvoiceCustomField(
            InvoiceCustomFieldId id,
            InvoiceId invoiceId,
            CustomFieldId customFieldId,
            string value)
            : base(id)
        {
            InvoiceId = invoiceId;
            CustomFieldId = customFieldId;
            Value = value;
        }

        /// <summary>
        /// Gets or sets the foreign key referencing the parent Invoice entity.
        /// </summary>
        public InvoiceId InvoiceId { private set; get; } = null!;

        /// <summary>
        /// Gets or sets the foreign key referencing the definition of the custom field (<see cref="CustomField"/>).
        /// </summary>
        public CustomFieldId CustomFieldId { private set; get; } = null!;

        /// <summary>
        /// Gets or sets the actual value entered by the user for this custom field instance.
        /// The type is <see cref="object"/> because the underlying type is defined by the associated <see cref="CustomField.Type"/>.
        /// </summary>
        public string Value { private set; get; } = null!;

        /// <summary>
        /// Creates a new instance of an invoice custom field.
        /// This is internal to ensure only the Invoice aggregate can create it,
        /// after it has performed the necessary type validation.
        /// <summary>
        /// Creates a new InvoiceCustomField for the specified invoice and custom field after validating the value's length.
        /// </summary>
        /// <param name="invoiceId">Identifier of the invoice the custom field value belongs to.</param>
        /// <param name="customFieldId">Identifier of the custom field definition.</param>
        /// <param name="value">The value to store; must be at most <c>Constants.MAX_CUSTOM_FIELD_VALUE_CHARACTERS</c> characters long.</param>
        /// <returns>
        /// A <see cref="Result{InvoiceCustomField}"/> containing the created entity on success; a failure result with
        /// <see cref="InvoiceCustomFieldErrors.ValueTooLong"/> if <paramref name="value"/> exceeds the maximum allowed length.
        /// </returns>
        internal static Result<InvoiceCustomField> Create(InvoiceId invoiceId, CustomFieldId customFieldId, string value)
        {
            if (value.Length > Constants.MAX_CUSTOM_FIELD_VALUE_CHARACTERS)
            {
                return Result.Failure<InvoiceCustomField>(InvoiceCustomFieldErrors.ValueTooLong);
            }

            // Further validation happens in the Invoice aggregate
            return Result.Success(new InvoiceCustomField(new InvoiceCustomFieldId(0), invoiceId, customFieldId, value));
        }

        /// <summary>
        /// Updates the value of the custom field.
        /// This method is internal to ensure only the parent Invoice aggregate can call it,
        ///  allowing the aggregate to enforce rules (e.g., cannot modify a sent invoice).
        /// <summary>
        /// Updates the stored value for the custom field after validating its length.
        /// </summary>
        /// <param name="newValue">The new value to assign; must not exceed Constants.MAX_CUSTOM_FIELD_VALUE_CHARACTERS characters.</param>
        /// <returns>A successful Result when the value is updated, or a failure Result containing InvoiceCustomFieldErrors.ValueTooLong if the new value exceeds the maximum allowed length.</returns>
        internal Result UpdateValue(string newValue)
        {
            if (newValue.Length > Constants.MAX_CUSTOM_FIELD_VALUE_CHARACTERS)
            {
                return Result.Failure(InvoiceCustomFieldErrors.ValueTooLong);
            }

            Value = newValue;
            return Result.Success();
        }
    }
}