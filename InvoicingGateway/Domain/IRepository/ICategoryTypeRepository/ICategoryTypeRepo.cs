using Domain.Entities;
using Domain.Models.Base;
using Domain.Utils.ResultUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepository.ICategoryTypeRepository
{
    public interface ICategoryTypeRepo
    {
        Task<ResultRepo<object>> AddAsync(CategoryType category);
        Task<ResultRepo<object>> UpdateAsync(int id, CategoryType category);
        Task<string> UpdateStatus(int id, int status);
        Task<string> DeleteAsync(int id);
        Task<IList<CategoryType>> GetAllAsync();
        Task<IList<CategoryType>> GetAllAsync(GeneralFilterDto generalFilter);
        Task<IReadOnlyList<CategoryType>> GetAllAsync(int pageNumber, int pageSize, object _oDataOptions);
        Task<CategoryType> GetByIdAsync(int id);
        Task<long> GetCountAsync(object _oDataOptions);
    }
}
