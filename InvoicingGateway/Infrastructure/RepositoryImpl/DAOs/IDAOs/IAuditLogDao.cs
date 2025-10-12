using Domain.Entities;
using Domain.Models.Base;
using Infrastructure.RepositoryImpl.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.RepositoryImpl.DAOs.IDAOs
{
    public interface IAuditLogDao
    {
        Task<bool> InsertAsync(AuditLog mdl, DataAccessRepository daRpst);
        Task<DataTable> GetAll(GeneralFilterDto generalFilter, DataAccessRepository daRpst);
    }
}
