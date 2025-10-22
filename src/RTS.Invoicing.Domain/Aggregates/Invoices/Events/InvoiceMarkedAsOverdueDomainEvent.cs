using RTS.Invoicing.Domain.Common;
using System;

namespace RTS.Invoicing.Domain.Aggregates.Invoices.Events
{
    /// <summary>
    /// Represents a domain event that is raised when an invoice's status
    /// is officially changed to 'Overdue'.
    /// </summary>
    /// <param name="InvoiceId">The ID of the invoice that is now overdue.</param>
    /// <param name="DueDate">The date the invoice was due.</param>
    public sealed record InvoiceMarkedAsOverdueDomainEvent(
        InvoiceId InvoiceId,
        DateTime DueDate)
        : BaseDomainEvent(Guid.NewGuid());
}
