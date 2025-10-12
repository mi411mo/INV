using Domain.Entities;
using Domain.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepository
{
    public interface IAuditLogRepo
    {
        Task<bool> AddAuditLogAsync(AuditLog auditLog);
        Task<IList<AuditLog>> GetAllAsync(GeneralFilterDto generalFilter);
    }
}
