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
    public interface ICategoryTypeService
    {
        Task<ResultRepo<object>> AddAsync(CategoryType category);
        Task<ResultRepo<object>> UpdateAsync(int id, CategoryType category);
        Task<string> UpdateStatusAsync(int id, int status);
        Task<string> DeleteAsync(int id);
        Task<IList<CategoryType>> GetAllAsync();
        Task<IList<CategoryType>> GetAllAsync(GeneralFilterDto generalFilter);
        Task<ServiceResult<PagedListDto<CategoryType>>> GetAllAsync(PaginationFilter filter, object oDataOptions);
        Task<CategoryType> GetByIdAsync(int id);
        Task<int> GetCountAsync(GeneralFilterDto generalFilter);
    }
}
