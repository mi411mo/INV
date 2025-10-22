using RTS.Invoicing.Domain.Contracts.Repositories.Base;
using RTS.Invoicing.Domain.Aggregates.Payments;

namespace RTS.Invoicing.Domain.Contracts.Repositories
{
    /// <summary>
    /// Defines a contract for a repository that manages <see cref="Payment"/> aggregates.
    /// </summary>
    /// <remarks>This interface extends <see cref="IRepository{T}"/> with <see cref="Payment"/> as the entity
    /// type, providing a standard set of data access operations for payment-related entities.</remarks>
    public interface IPaymentRepository : IRepository<Payment>
    {
    }
}
