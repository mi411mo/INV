using RTS.Invoicing.Domain.Contracts.Repositories.Base;
using RTS.Invoicing.Domain.Aggregates.Customers;

namespace RTS.Invoicing.Domain.Contracts.Repositories
{
    /// <summary>
    /// Defines a repository for managing <see cref="Person"/> entities.
    /// </summary>
    /// <remarks>This interface extends <see cref="IRepository{T}"/> to provide functionality specific to <see
    /// cref="Person"/> entities. It serves as a contract for implementing data access operations related to <see
    /// cref="Person"/> objects.</remarks>
    public interface IPersonRepository : IRepository<Person>
    {
    }
}
