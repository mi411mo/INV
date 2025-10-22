using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.ValueObjects;
using System;

namespace RTS.Invoicing.Domain.Entities.Invoices.Events
{
    public sealed record InvoiceItemUpdatedDomainEvent(
        InvoiceId InvoiceId,
        InvoiceItemId InvoiceItemId,
        Money TotalAmount)
        : BaseDomainEvent(Guid.NewGuid());
}
