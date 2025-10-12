using Domain.Entities;
using Domain.IRepository;
using Domain.Models.Base;
using Domain.Utils.ResultUtils;
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
    public class LogRepoImpl : ILogRepo
    {
        private readonly DataAccessRepository dar;
        private ILogDao logDao;
        public LogRepoImpl(DataAccessRepository _dar, ILogDao logDao)
        {
            dar = _dar;
            this.logDao = logDao;
        }
        public async Task<ResultRepo<object>> AddAsync(Log log)
        {
            long id = 0;
            var result = await logDao.InsertAsync(log, dar);
            return ReturnRepo.Success(id);
        }

        public async Task<IList<Log>> GetAllAsync()
        {
            var dt = await logDao.GetAll(dar);
            var lst = ConvertToModel<Log>.DataTableToModels(dt);
            return lst;
        }

        public async Task<IList<Log>> GetAllAsync(GeneralFilterDto generalFilter)
        {
            var dt = await logDao.GetAll(generalFilter, dar);
            var logs = ConvertToModel<Log>.DataTableToModels(dt);
            return logs;
        }

        public async Task<Log> GetByIdAsync(int id)
        {
            var dt = await logDao.GetById(id, dar);
            var lst = ConvertToModel<Log>.DataTableToModels(dt);
            var logData = lst.FirstOrDefault();
            return logData;
        }
    }
}
