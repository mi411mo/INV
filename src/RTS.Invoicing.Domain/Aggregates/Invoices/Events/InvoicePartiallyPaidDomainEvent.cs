using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.ValueObjects;
using System;

namespace RTS.Invoicing.Domain.Aggregates.Invoices.Events
{
    /// <summary>
    /// Represents a domain event that is raised when an invoice receives a partial payment.
    /// </summary>
    /// <param name="InvoiceId">The ID of the invoice that was partially paid.</param>
    /// <param name="AmountPaid">The amount that was just paid.</param>
    /// <param name="AmountRemaining">The new remaining balance due on the invoice.</param>
    /// <param name="DueDate">The due date of the invoice.</param>
    public sealed record InvoicePartiallyPaidDomainEvent(
        InvoiceId InvoiceId,
        Money AmountPaid,
        Money AmountRemaining,
        DateTime DueDate)
    : BaseDomainEvent(Guid.NewGuid());
}
