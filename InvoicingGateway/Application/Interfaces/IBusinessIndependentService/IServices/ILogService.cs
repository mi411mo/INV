using Domain.Entities;
using Domain.Models.Base;
using Domain.Utils.ResultUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IBusinessIndependentService.IServices
{
    public interface ILogService
    {
        Task<ResultRepo<object>> AddAsync(Log log);
        Task<IList<Log>> GetAllAsync();
        Task<IList<Log>> GetAllAsync(GeneralFilterDto generalFilter);
        Task<Log> GetByIdAsync(int id);
        Task<int> GetCountAsync(GeneralFilterDto generalFilter);
    }
}
