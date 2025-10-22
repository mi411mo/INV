using RTS.Invoicing.Domain.Common;
using System;

namespace RTS.Invoicing.Domain.Aggregates.Merchants.Events
{
    /// <summary>
    /// Represents a domain event that is raised when a merchant's custom field
    /// definition (e.g., its name or type) is updated.
    /// </summary>
    /// <param name="MerchantId">The ID of the merchant who owns the custom field.</param>
    /// <param name="CustomFieldId">The ID of the custom field definition that was updated.</param>
    public sealed record MerchantCustomFieldUpdatedDomainEvent(
        MerchantId MerchantId,
        CustomFieldId CustomFieldId)
        : BaseDomainEvent(Guid.NewGuid());
}
