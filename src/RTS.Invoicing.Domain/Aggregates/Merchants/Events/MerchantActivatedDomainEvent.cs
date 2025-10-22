using RTS.Invoicing.Domain.Common;
using System;

namespace RTS.Invoicing.Domain.Aggregates.Merchants.Events
{
    /// <summary>
    /// Represents a domain event that is raised when a merchant's account is activated.
    /// </summary>
    /// <param name="MerchantId">The ID of the merchant who was activated.</param>
    public sealed record MerchantActivatedDomainEvent(
        MerchantId MerchantId)
        : BaseDomainEvent(Guid.NewGuid());
}
