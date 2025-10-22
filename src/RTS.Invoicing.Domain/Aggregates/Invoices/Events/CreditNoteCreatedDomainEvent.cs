using RTS.Invoicing.Domain.Common;
using System;

namespace RTS.Invoicing.Domain.Aggregates.Invoices.Events
{
    /// <summary>
    /// Represents a domain event that is raised when a new credit note is created.
    /// </summary>
    /// <param name="InvoiceId">The ID of the newly created credit note (which is also an invoice).</param>
    /// <param name="OriginalInvoiceId">The ID of the original invoice that this credit note is issued against.</param>
    public sealed record CreditNoteCreatedDomainEvent(
        InvoiceId InvoiceId,
        InvoiceId OriginalInvoiceId)
        : BaseDomainEvent(Guid.NewGuid());
}
