using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.ValueObjects;
using System;

namespace RTS.Invoicing.Domain.Aggregates.Invoices.Events
{
    /// <summary>
    /// Represents a domain event that is raised when a new item is added to an invoice.
    /// </summary>
    /// <param name="InvoiceId">The ID of the invoice to which the item was added.</param>
    /// <param name="InvoiceItemId">The ID of the newly added invoice item.</param>
    /// <param name="TotalAmount">The total monetary value of the added item.</param>
    public sealed record InvoiceItemAddedDomainEvent(
        InvoiceId InvoiceId,
        InvoiceItemId InvoiceItemId,
        Money TotalAmount)
        : BaseDomainEvent(Guid.NewGuid());
}
