using RTS.Invoicing.Domain.Contracts.Repositories.Base;
using RTS.Invoicing.Domain.Aggregates.Customers;

namespace RTS.Invoicing.Domain.Contracts.Repositories
{
    /// <summary>
    /// Defines a contract for a repository that provides data access operations for <see cref="Customer"/> aggregate.
    /// </summary>
    /// <remarks>This interface extends the <see cref="IRepository{T}"/> interface, inheriting its generic
    /// data access methods  and specializing them for <see cref="Customer"/> entities. Implementations of this
    /// interface are responsible  for managing the persistence and retrieval of customer data.</remarks>
    public interface ICustomerRepository : IRepository<Customer>
    {
    }
}
