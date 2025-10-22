using RTS.Invoicing.Domain.Common;
using System;

namespace RTS.Invoicing.Domain.Entities.Merchants.Events
{
    public sealed record MerchantDeactivatedDomainEvent(
        MerchantId MerchantId)
        : BaseDomainEvent(Guid.NewGuid());
}
