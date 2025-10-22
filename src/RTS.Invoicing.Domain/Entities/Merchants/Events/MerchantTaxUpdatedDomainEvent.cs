using RTS.Invoicing.Domain.Common;
using System;

namespace RTS.Invoicing.Domain.Entities.Merchants.Events
{
    public sealed record MerchantTaxUpdatedDomainEvent(
        MerchantId MerchantId,
        TaxId Taxid,
        string NewName,
        string NewCode,
        decimal NewRate)
        : BaseDomainEvent(Guid.NewGuid());
}
