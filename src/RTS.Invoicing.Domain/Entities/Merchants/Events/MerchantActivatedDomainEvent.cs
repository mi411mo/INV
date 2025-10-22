using RTS.Invoicing.Domain.Common;
using System;

namespace RTS.Invoicing.Domain.Entities.Merchants.Events
{
    public sealed record MerchantActivatedDomainEvent(
        MerchantId MerchantId)
        : BaseDomainEvent(Guid.NewGuid());
}
