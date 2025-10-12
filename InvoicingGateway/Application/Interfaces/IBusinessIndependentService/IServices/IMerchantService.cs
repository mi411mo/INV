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
    public interface IMerchantService
    {
        Task<ResultRepo<object>> AddAsync(Merchant merchant);
        Task<ResultRepo<object>> UpdateAsync(int id, Merchant merchant);
        Task<string> UpdateStatusAsync(int id, int status);
        Task<string> DeleteAsync(int id);
        Task<IList<Merchant>> GetAllAsync();
        Task<IList<Merchant>> GetAllAsync(GeneralFilterDto generalFilter);
        Task<ServiceResult<PagedListDto<Merchant>>> GetAllAsync(PaginationFilter filter, object oDataOptions);
        Task<Merchant> GetByIdAsync(int id);
        Task<int> GetCountAsync(GeneralFilterDto generalFilter);
    }
}
