using RTS.Invoicing.Domain.Common;
using System;

namespace RTS.Invoicing.Domain.Aggregates.Merchants.Events
{
    /// <summary>
    /// Represents a domain event that is raised when a merchant's configuration is updated.
    /// </summary>
    /// <param name="MerchantId">The ID of the merchant whose configuration was updated.</param>
    /// <param name="NewConfiguration">A string (e.g., JSON) representing the new configuration settings.</param>
    public sealed record MerchantConfigurationUpdatedDomainEvent(
        MerchantId MerchantId,
        string NewConfiguration)
        : BaseDomainEvent(Guid.NewGuid());
}
