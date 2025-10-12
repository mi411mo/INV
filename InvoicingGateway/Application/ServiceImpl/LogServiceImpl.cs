using Application.Interfaces.IBusinessIndependentService.IServices;
using Domain.Entities;
using Domain.IRepository;
using Domain.Models.Base;
using Domain.Utils.ResultUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceImpl
{
    public class LogServiceImpl : ILogService
    {
        private readonly ILogRepo logRepo;
        public LogServiceImpl(ILogRepo logRepo)
        {
            this.logRepo = logRepo;
        }
        public async Task<ResultRepo<object>> AddAsync(Log log)
        {
            ResultRepo<object> res = new ResultRepo<object>();
            try
            {
                res = await logRepo.AddAsync(log);
            }
            catch (Exception ex)
            {
                throw;
            }
            return res;
        }

        public async Task<IList<Log>> GetAllAsync()
        {
            try
            {
                var lst = await logRepo.GetAllAsync();
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IList<Log>> GetAllAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                var lst = await logRepo.GetAllAsync(generalFilter);
                return lst.Skip((generalFilter.PageNumber - 1) * generalFilter.PageSize)
                .Take(generalFilter.PageSize)
                .OrderByDescending(e => e.Id)
                .ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Log> GetByIdAsync(int id)
        {
            try
            {
                var logData = await logRepo.GetByIdAsync(id);
                return logData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> GetCountAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                var lst = await logRepo.GetAllAsync(generalFilter);
                return lst.Count();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
