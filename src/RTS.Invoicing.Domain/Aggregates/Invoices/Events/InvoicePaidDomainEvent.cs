using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.ValueObjects;
using System;

namespace RTS.Invoicing.Domain.Aggregates.Invoices.Events
{
    /// <summary>
    /// Represents a domain event that is raised when an invoice is fully paid.
    /// </summary>
    /// <param name="InvoiceId">The ID of the invoice that was paid.</param>
    /// <param name="AmountPaid">The final payment amount that resulted in the invoice being fully paid.</param>
    public sealed record InvoicePaidDomainEvent(
        InvoiceId InvoiceId,
        Money AmountPaid)
    : BaseDomainEvent(Guid.NewGuid());
}
