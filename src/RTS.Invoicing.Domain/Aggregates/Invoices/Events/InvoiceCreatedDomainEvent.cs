using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.Aggregates.Customers;
using System;

namespace RTS.Invoicing.Domain.Aggregates.Invoices.Events
{
    /// <summary>
    /// Represents a domain event that is raised when a new invoice is created.
    /// </summary>
    /// <param name="InvoiceId">The ID of the newly created invoice.</param>
    /// <param name="CustomerId">The ID of the customer the invoice was created for.</param>
    public sealed record InvoiceCreatedDomainEvent(
        InvoiceId InvoiceId,
        CustomerId CustomerId)
        : BaseDomainEvent(Guid.NewGuid());
}
