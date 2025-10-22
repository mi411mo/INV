using RTS.Invoicing.Domain.Common;
using System;

namespace RTS.Invoicing.Domain.Entities.Merchants.Events
{
    public sealed record MerchantDefaultCurrencyChangedDomainEvent(
        MerchantId MerchantId,
        string NewDefaultCurrencyCode)
        : BaseDomainEvent(Guid.NewGuid());
}
