using RTS.Invoicing.Domain.Contracts.Repositories.Base;
using RTS.Invoicing.Domain.Entities.AuditLogs;

namespace RTS.Invoicing.Domain.Contracts.Repositories
{
    public interface IAuditLogRepository : IRepository<AuditLog>
    {
    }
}
