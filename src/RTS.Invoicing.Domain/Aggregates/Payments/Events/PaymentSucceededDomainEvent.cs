using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.Aggregates.Invoices;
using RTS.Invoicing.Domain.Aggregates.Payments;
using RTS.Invoicing.Domain.ValueObjects;
using System;

namespace RTS.Invoicing.Domain.Aggregates.Merchants.Events
{
    /// <summary>
    /// Represents a domain event that is raised when a payment is successfully processed.
    /// </summary>
    /// <param name="PaymentId">The ID of the payment that succeeded.</param>
    /// <param name="InvoiceId">The ID of the invoice that the payment was for.</param>
    /// <param name="Amount">The monetary amount that was successfully paid.</param>
    public sealed record PaymentSucceededDomainEvent(
        PaymentId PaymentId,
        InvoiceId InvoiceId,
        Money Amount)
        : BaseDomainEvent(Guid.NewGuid());
}
