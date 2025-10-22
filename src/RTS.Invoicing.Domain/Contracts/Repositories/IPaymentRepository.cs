using RTS.Invoicing.Domain.Contracts.Repositories.Base;
using RTS.Invoicing.Domain.Entities.Payments;

namespace RTS.Invoicing.Domain.Contracts.Repositories
{
    public interface IPaymentRepository : IRepository<Payment>
    {
    }
}
