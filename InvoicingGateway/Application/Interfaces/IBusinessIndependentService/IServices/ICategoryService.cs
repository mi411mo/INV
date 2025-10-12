using Domain.Entities;
using Domain.Models;
using Domain.Models.Base;
using Domain.Utils.ResultUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IBusinessIndependentService.IServices
{
    public interface ICategoryService
    {
        Task<ResultRepo<object>> AddAsync(Category category);
        Task<ResultRepo<object>> UpdateAsync(int id, Category category);
        Task<string> UpdateStatusAsync(int id, int status);
        Task<string> DeleteAsync(int id);
        Task<IList<Category>> GetAllAsync();
        Task<IList<Category>> GetAllAsync(GeneralFilterDto generalFilter);
        Task<ServiceResult<PagedListDto<Category>>> GetAllAsync(PaginationFilter filter, object oDataOptions);
        Task<Category> GetByIdAsync(int id);
        Task<int> GetCountAsync(GeneralFilterDto generalFilter);
    }
}
