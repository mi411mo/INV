using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.Entities.Invoices;
using RTS.Invoicing.Domain.Entities.Payments;
using System;

namespace RTS.Invoicing.Domain.Entities.Merchants.Events
{
    public sealed record PaymentFailedDomainEvent(
        PaymentId PaymentId,
        InvoiceId InvoiceId,
        string Reason)
        : BaseDomainEvent(Guid.NewGuid());
}
