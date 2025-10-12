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
    public interface ICategoryRepo
    {
        Task<ResultRepo<object>> AddAsync(Category category);
        Task<ResultRepo<object>> UpdateAsync(int id, Category category);
        Task<string> UpdateStatus(int id, int status);
        Task<string> DeleteAsync(int id);
        Task<IList<Category>> GetAllAsync();
        Task<IList<Category>> GetAllAsync(GeneralFilterDto generalFilter);
        Task<IReadOnlyList<Category>> GetAllAsync(int pageNumber, int pageSize, object _oDataOptions);
        Task<Category> GetByIdAsync(int id);
        Task<long> GetCountAsync(object _oDataOptions);
    }
}
