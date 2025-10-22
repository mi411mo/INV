using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.ValueObjects;
using System;

namespace RTS.Invoicing.Domain.Aggregates.Invoices.Events
{
    /// <summary>
    /// Represents a domain event that is raised when an item is removed from an invoice.
    /// </summary>
    /// <param name="InvoiceId">The ID of the invoice from which the item was removed.</param>
    /// <param name="InvoiceItemId">The ID of the invoice item that was removed.</param>
    /// <param name="TotalAmount">The total monetary value of the removed item (which will be subtracted from the invoice).</param>
    public sealed record InvoiceItemRemovedDomainEvent(
        InvoiceId InvoiceId,
        InvoiceItemId InvoiceItemId,
        Money TotalAmount)
        : BaseDomainEvent(Guid.NewGuid());
}
