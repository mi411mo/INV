using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.Aggregates.Customers;
using System;

namespace RTS.Invoicing.Domain.Aggregates.Invoices.Events
{
    /// <summary>
    /// Represents a domain event that is raised when an invoice is sent to a customer.
    /// </summary>
    /// <param name="InvoiceId">The ID of the invoice that was sent.</param>
    /// <param name="CustomerId">The ID of the customer to whom the invoice was sent.</param>
    /// <param name="DueDate">The date the invoice is due.</param>
    public sealed record InvoiceSentDomainEvent(
        InvoiceId InvoiceId,
        CustomerId CustomerId,
        DateTime DueDate)
        : BaseDomainEvent(Guid.NewGuid());
}
