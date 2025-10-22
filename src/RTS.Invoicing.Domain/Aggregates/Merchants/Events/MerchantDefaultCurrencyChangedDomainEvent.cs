using RTS.Invoicing.Domain.Common;
using System;

namespace RTS.Invoicing.Domain.Aggregates.Merchants.Events
{
    /// <summary>
    /// Represents a domain event that is raised when a merchant's
    /// default currency is changed.
    /// </summary>
    /// <param name="MerchantId">The ID of the merchant whose default currency was changed.</param>
    /// <param name="NewDefaultCurrencyCode">The new default ISO 4217 currency code.</param>
    public sealed record MerchantDefaultCurrencyChangedDomainEvent(
        MerchantId MerchantId,
        string NewDefaultCurrencyCode)
        : BaseDomainEvent(Guid.NewGuid());
}
