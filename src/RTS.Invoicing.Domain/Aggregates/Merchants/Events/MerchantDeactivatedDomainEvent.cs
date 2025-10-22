using RTS.Invoicing.Domain.Common;
using System;

namespace RTS.Invoicing.Domain.Aggregates.Merchants.Events
{
    /// <summary>
    /// Represents a domain event that is raised when a merchant's account is deactivated.
    /// </summary>/// <param name="MerchantId">The ID of the merchant who was deactivated.</param>
    public sealed record MerchantDeactivatedDomainEvent(
        MerchantId MerchantId)
        : BaseDomainEvent(Guid.NewGuid());
}
