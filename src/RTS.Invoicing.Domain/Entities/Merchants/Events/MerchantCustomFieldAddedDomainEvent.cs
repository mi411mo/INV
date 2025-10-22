using RTS.Invoicing.Domain.Common;
using System;

namespace RTS.Invoicing.Domain.Entities.Merchants.Events
{
    public sealed record MerchantCustomFieldAddedDomainEvent(
        MerchantId MerchantId,
        CustomFieldId CustomFieldId)
        : BaseDomainEvent(Guid.NewGuid());
}
