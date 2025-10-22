using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.Entities.Customers;
using System;

namespace RTS.Invoicing.Domain.Entities.Invoices.Events
{
    public sealed record InvoiceCreatedDomainEvent(
        InvoiceId InvoiceId,
        CustomerId CustomerId)
        : BaseDomainEvent(Guid.NewGuid());
}
