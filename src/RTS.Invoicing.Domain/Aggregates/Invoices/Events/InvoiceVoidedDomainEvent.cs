using RTS.Invoicing.Domain.Common;
using System;

namespace RTS.Invoicing.Domain.Aggregates.Invoices.Events
{
    /// <summary>
    /// Represents a domain event that is raised when an invoice is voided (cancelled).
    /// </summary>
    /// <param name="InvoiceId">The ID of the invoice that was voided.</param>
    /// <param name="DueDate">The original due date of the voided invoice.</param>
    public sealed record InvoiceVoidedDomainEvent(
        InvoiceId InvoiceId,
        DateTime DueDate)
        : BaseDomainEvent(Guid.NewGuid());
}
