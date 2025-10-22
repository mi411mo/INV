using RTS.Invoicing.Domain.Common;
using System;

namespace RTS.Invoicing.Domain.Entities.Invoices.Events
{
    public sealed record CreditNoteCreatedDomainEvent(
        InvoiceId InvoiceId,
        InvoiceId OriginalInvoiceId)
        : BaseDomainEvent(Guid.NewGuid());
}
