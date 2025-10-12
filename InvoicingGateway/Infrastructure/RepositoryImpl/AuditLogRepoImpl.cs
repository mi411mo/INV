using Domain.Entities;
using Domain.IRepository;
using Domain.Models.Base;
using Infrastructure.Repositories.Impl.V1.DRY;
using Infrastructure.RepositoryImpl.DAL;
using Infrastructure.RepositoryImpl.DAOs.IDAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.RepositoryImpl
{
    public class AuditLogRepoImpl : IAuditLogRepo
    {
        private readonly DataAccessRepository dar;
        private IAuditLogDao auditLogDao;
        public AuditLogRepoImpl(DataAccessRepository _dar, IAuditLogDao auditLogDao)
        {
            dar = _dar;
            this.auditLogDao = auditLogDao;
        }
        public async Task<bool> AddAuditLogAsync(AuditLog auditLog)
        {
            try
            {
                long id = 0;
                var isCreated = await auditLogDao.InsertAsync(auditLog, dar);
                return isCreated;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IList<AuditLog>> GetAllAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                var dt = await auditLogDao.GetAll(generalFilter, dar);
                var auditLogs = ConvertToModel<AuditLog>.DataTableToModels(dt);
                return auditLogs;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
