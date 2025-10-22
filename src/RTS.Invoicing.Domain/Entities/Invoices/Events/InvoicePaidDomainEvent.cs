using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.ValueObjects;
using System;

namespace RTS.Invoicing.Domain.Entities.Invoices.Events
{
    public sealed record InvoicePaidDomainEvent(
        InvoiceId InvoiceId,
        Money AmountPaid)
    : BaseDomainEvent(Guid.NewGuid());
}
