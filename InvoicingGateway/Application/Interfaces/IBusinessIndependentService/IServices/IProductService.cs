using Domain.Entities;
using Domain.Models;
using Domain.Models.Base;
using Domain.Utils.ResultUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IProductService
    {
        Task<ResultRepo<object>> AddAsync(ProductModel serviceModel);
        Task<ResultRepo<object>> UpdateAsync(int id, ProductModel serviceModel);
        Task<string> UpdateStatusAsync(int id, int status);
        Task<string> DeleteAsync(int id);
        Task<IList<ProductModel>> GetAllAsync(GeneralFilterDto generalFilter);
        Task<ServiceResult<PagedListDto<ProductModel>>> GetAllAsync(PaginationFilter filter, object oDataOptions);
        Task<ProductModel> GetByIdAsync(int id);
        Task<int> GetCountAsync(GeneralFilterDto generalFilter);
    }
}
