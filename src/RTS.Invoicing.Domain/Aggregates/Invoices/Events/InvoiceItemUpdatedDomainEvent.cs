using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.ValueObjects;
using System;

namespace RTS.Invoicing.Domain.Aggregates.Invoices.Events
{
    /// <summary>
    /// Represents a domain event that is raised when an existing invoice item is updated.
    /// </summary>
    /// <param name="InvoiceId">The ID of the invoice containing the updated item.</param>
    /// <param name="InvoiceItemId">The ID of the invoice item that was updated.</param>
    /// <param name="TotalAmount">The new total monetary value of the updated item.</param>
    public sealed record InvoiceItemUpdatedDomainEvent(
        InvoiceId InvoiceId,
        InvoiceItemId InvoiceItemId,
        Money TotalAmount)
        : BaseDomainEvent(Guid.NewGuid());
}
