using RTS.Invoicing.Domain.Common;
using System;

namespace RTS.Invoicing.Domain.Entities.Merchants.Events
{
    public sealed record MerchantTaxAddedEvent(
        MerchantId MerchantId,
        TaxId Taxid,
        string Code,
        decimal Rate)
        : BaseDomainEvent(Guid.NewGuid());
}
