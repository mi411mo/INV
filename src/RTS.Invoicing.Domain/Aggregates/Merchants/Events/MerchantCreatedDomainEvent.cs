using RTS.Invoicing.Domain.Common;
using System;

namespace RTS.Invoicing.Domain.Aggregates.Merchants.Events
{
    /// <summary>
    /// Represents a domain event that is raised when a new merchant is created.
    /// </summary>
    /// <param name="merchantId">The ID of the newly created merchant.</param>
    /// <param name="englishName">The English name of the merchant.</param>
    /// <param name="defaultCurrencyCode">The default ISO 4217 currency code for the merchant.</param>
    public sealed record MerchantCreatedDomainEvent(
        MerchantId merchantId,
        string englishName,
        string defaultCurrencyCode)
        : BaseDomainEvent(Guid.NewGuid());

}
