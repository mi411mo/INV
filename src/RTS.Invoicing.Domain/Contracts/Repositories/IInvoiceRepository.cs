using RTS.Invoicing.Domain.Contracts.Repositories.Base;
using RTS.Invoicing.Domain.Aggregates.Invoices;

namespace RTS.Invoicing.Domain.Contracts.Repositories
{
    /// <summary>
    /// Defines a repository for managing <see cref="Invoice"/> aggreagates, providing methods for data access and
    /// manipulation.
    /// </summary>
    /// <remarks>This interface extends <see cref="IRepository{T}"/> with <see cref="Invoice"/> as the entity
    /// type, inheriting its generic repository functionality. It serves as a contract for implementing data access
    /// logic specific to invoices.</remarks>
    public interface IInvoiceRepository : IRepository<Invoice>
    {
    }
}
