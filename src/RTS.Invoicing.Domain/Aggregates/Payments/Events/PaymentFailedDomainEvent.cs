using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.Aggregates.Invoices;
using RTS.Invoicing.Domain.Aggregates.Payments;
using System;

namespace RTS.Invoicing.Domain.Aggregates.Merchants.Events
{
    /// <summary>
    /// Represents a domain event that is raised when a payment processing attempt fails.
    /// </summary>
    /// <param name="PaymentId">The ID of the payment that failed.</param>
    /// <param name="InvoiceId">The ID of the invoice associated with the failed payment.</param>
    /// <param name="Reason">A description of why the payment failed.</param>
    public sealed record PaymentFailedDomainEvent(
        PaymentId PaymentId,
        InvoiceId InvoiceId,
        string Reason)
        : BaseDomainEvent(Guid.NewGuid());
}
