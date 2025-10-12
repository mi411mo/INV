using Domain.Entities;
using Domain.Models.Base;
using Domain.Utils.ResultUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepository
{
    public interface ILogRepo
    {
        Task<ResultRepo<object>> AddAsync(Log log);
        Task<IList<Log>> GetAllAsync();
        Task<IList<Log>> GetAllAsync(GeneralFilterDto generalFilter);
        Task<Log> GetByIdAsync(int id);
    }
}
