using RTS.Invoicing.Domain.Contracts.Repositories.Base;
using RTS.Invoicing.Domain.Aggregates.Merchants;

namespace RTS.Invoicing.Domain.Contracts.Repositories
{
    /// <summary>
    /// Defines a repository for managing <see cref="Merchant"/> aggregates, providing methods for data access and
    /// manipulation.
    /// </summary>
    /// <remarks>This interface extends <see cref="IRepository{T}"/> with <see cref="Merchant"/> as the entity
    /// type, enabling operations such as querying, adding, updating, and deleting merchants in the underlying data
    /// store.</remarks>
    public interface IMerchantRepository : IRepository<Merchant>
    {
    }
}
