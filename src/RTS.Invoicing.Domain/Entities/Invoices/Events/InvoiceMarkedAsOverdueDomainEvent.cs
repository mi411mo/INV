using RTS.Invoicing.Domain.Common;
using System;

namespace RTS.Invoicing.Domain.Entities.Invoices.Events
{
    public sealed record InvoiceMarkedAsOverdueDomainEvent(
        InvoiceId InvoiceId,
        DateTime DueDate)
        : BaseDomainEvent(Guid.NewGuid());
}
