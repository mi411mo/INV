using RTS.Invoicing.Domain.Contracts.Repositories.Base;
using RTS.Invoicing.Domain.Entities.Invoices;

namespace RTS.Invoicing.Domain.Contracts.Repositories
{
    public interface IInvoiceRepository : IRepository<Invoice>
    {
    }
}
