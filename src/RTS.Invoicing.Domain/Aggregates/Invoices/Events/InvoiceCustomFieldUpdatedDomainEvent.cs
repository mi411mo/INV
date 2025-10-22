using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.Aggregates.Merchants;
using System;

namespace RTS.Invoicing.Domain.Aggregates.Invoices.Events
{
    /// <summary>
    /// Represents a domain event that is raised when a custom field's value
    /// on an existing invoice is updated.
    /// </summary>
    /// <param name="InvoiceId">The ID of the invoice that was updated.</param>
    /// <param name="CustomFieldId">The ID of the custom field that was updated.</param>
    /// <param name="Value">The new value assigned to the custom field.</param>
    public sealed record InvoiceCustomFieldUpdatedDomainEvent(
        InvoiceId InvoiceId,
        CustomFieldId CustomFieldId,
        string Value)
        : BaseDomainEvent(Guid.NewGuid());
}
