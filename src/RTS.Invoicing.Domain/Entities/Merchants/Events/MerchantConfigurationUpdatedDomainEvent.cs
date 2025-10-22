using RTS.Invoicing.Domain.Common;
using System;

namespace RTS.Invoicing.Domain.Entities.Merchants.Events
{
    public sealed record MerchantConfigurationUpdatedDomainEvent(
        MerchantId MerchantId,
        string NewConfiguration)
        : BaseDomainEvent(Guid.NewGuid());
}
