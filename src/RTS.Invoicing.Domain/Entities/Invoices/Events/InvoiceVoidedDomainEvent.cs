using RTS.Invoicing.Domain.Common;
using System;

namespace RTS.Invoicing.Domain.Entities.Invoices.Events
{
    public sealed record InvoiceVoidedDomainEvent(
        InvoiceId invoiceId,
        DateTime DueDate)
        : BaseDomainEvent(Guid.NewGuid());
}
