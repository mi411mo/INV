using RTS.Invoicing.Domain.Common;
using System;

namespace RTS.Invoicing.Domain.Aggregates.Merchants.Events
{
    /// <summary>
    /// Represents a domain event that is raised when a merchant's existing tax
    /// definition is updated.
    /// </summary>
    /// <param name="MerchantId">The ID of the merchant who owns the tax definition.</param>
    /// <param name="Taxid">The ID of the tax definition that was updated.</param>
    /// <param name="NewName">The new name for the tax.</param>
    /// <param name="NewCode">The new code for the tax.</param>
    /// <param name="NewRate">The new percentage rate for the tax.</param>
    public sealed record MerchantTaxUpdatedDomainEvent(
        MerchantId MerchantId,
        TaxId Taxid,
        string NewName,
        string NewCode,
        decimal NewRate)
        : BaseDomainEvent(Guid.NewGuid());
}
