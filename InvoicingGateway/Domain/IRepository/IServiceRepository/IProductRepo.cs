using Domain.Entities;
using Domain.Models.Base;
using Domain.Utils.ResultUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepository.IServiceRepository
{
    public interface IProductRepo
    {
        Task<ResultRepo<object>> AddAsync(ProductModel serviceModel);
        Task<ResultRepo<object>> UpdateAsync(int id, ProductModel serviceModel);
        Task<string> UpdateStatus(int id, int status);
        Task<string> DeleteAsync(int id);
        Task<IList<ProductModel>> GetAllAsync(GeneralFilterDto generalFilter);
        Task<IReadOnlyList<ProductModel>> GetAllAsync(int pageNumber, int pageSize, object _oDataOptions);
 
        Task<ProductModel> GetByIdAsync(int id);
        Task<long> GetCountAsync(object _oDataOptions);
    }
}
