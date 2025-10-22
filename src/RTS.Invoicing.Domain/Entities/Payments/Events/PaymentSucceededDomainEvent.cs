using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.Entities.Invoices;
using RTS.Invoicing.Domain.Entities.Payments;
using RTS.Invoicing.Domain.ValueObjects;
using System;

namespace RTS.Invoicing.Domain.Entities.Merchants.Events
{
    public sealed record PaymentSucceededDomainEvent(
        PaymentId PaymentId,
        InvoiceId InvoiceId,
        Money Amount)
        : BaseDomainEvent(Guid.NewGuid());
}
