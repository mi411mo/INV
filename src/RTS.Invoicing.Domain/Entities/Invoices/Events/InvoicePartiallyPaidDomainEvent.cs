using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.ValueObjects;
using System;

namespace RTS.Invoicing.Domain.Entities.Invoices.Events
{
    public sealed record InvoicePartiallyPaidDomainEvent(
        InvoiceId InvoiceId,
        Money AmountPaid,
        Money AmountRemaining,
        DateTime DueDate)
    : BaseDomainEvent(Guid.NewGuid());
}
