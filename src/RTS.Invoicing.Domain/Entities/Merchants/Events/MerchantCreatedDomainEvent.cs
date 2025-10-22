using RTS.Invoicing.Domain.Common;
using System;

namespace RTS.Invoicing.Domain.Entities.Merchants.Events
{
    public sealed record MerchantCreatedDomainEvent(
        MerchantId merchantId,
        string englishName,
        string defaultCurrencyCode)
        : BaseDomainEvent(Guid.NewGuid());

}
