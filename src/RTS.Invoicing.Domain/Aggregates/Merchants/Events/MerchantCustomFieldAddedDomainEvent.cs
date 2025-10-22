using RTS.Invoicing.Domain.Common;
using System;

namespace RTS.Invoicing.Domain.Aggregates.Merchants.Events
{
    /// <summary>
    /// Represents a domain event that is raised when a new custom field
    /// definition is added to a merchant's account.
    /// </summary>
    /// <param name="MerchantId">The ID of the merchant to whom the custom field was added.</param>
    /// <param name="CustomFieldId">The ID of the newly created custom field definition.</param>
    public sealed record MerchantCustomFieldAddedDomainEvent(
        MerchantId MerchantId,
        CustomFieldId CustomFieldId)
        : BaseDomainEvent(Guid.NewGuid());
}
