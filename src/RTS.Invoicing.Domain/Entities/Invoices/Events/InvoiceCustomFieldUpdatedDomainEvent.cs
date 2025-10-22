using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.Entities.Merchants;
using System;

namespace RTS.Invoicing.Domain.Entities.Invoices.Events
{
    public sealed record InvoiceCustomFieldUpdatedDomainEvent(
        InvoiceId InvoiceId,
        CustomFieldId CustomFieldId,
        string Value)
        : BaseDomainEvent(Guid.NewGuid());
}
