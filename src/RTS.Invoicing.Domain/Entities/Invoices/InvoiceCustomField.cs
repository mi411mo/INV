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
        /// <param name="value">The value.</param>
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
        /// </summary>
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
        /// </summary>
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
