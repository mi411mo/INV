using RTS.Invoicing.Domain.Common;
using System;

namespace RTS.Invoicing.Domain.Aggregates.Merchants.Events
{
    /// <summary>
    /// Represents a domain event that is raised when a new tax
    /// definition is added to a merchant's account.
    /// </summary>
    /// <param name="MerchantId">The ID of the merchant to whom the tax was added.</param>
    /// <param name="Taxid">The ID of the newly created tax definition.</param>
    /// <param name="Code">The code assigned to the new tax (e.g., "VAT").</param>
    /// <param name="Rate">The percentage rate of the new tax.</param>
    public sealed record MerchantTaxAddedEvent(
        MerchantId MerchantId,
        TaxId Taxid,
        string Code,
        decimal Rate)
        : BaseDomainEvent(Guid.NewGuid());
}
