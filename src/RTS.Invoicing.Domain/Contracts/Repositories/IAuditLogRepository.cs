using RTS.Invoicing.Domain.Aggregates.AuditLogs;
using RTS.Invoicing.Domain.Contracts.Repositories.Base;

namespace RTS.Invoicing.Domain.Contracts.Repositories
{
    public interface IAuditLogRepository : IRepository<AuditLog>
    {
    }
}
