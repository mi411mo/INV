using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.Entities.Customers;
using System;

namespace RTS.Invoicing.Domain.Entities.Invoices.Events
{
    public sealed record InvoiceSentDomainEvent(
        InvoiceId InvoiceId,
        CustomerId CustomerId,
        DateTime DueDate)
        : BaseDomainEvent(Guid.NewGuid());
}
