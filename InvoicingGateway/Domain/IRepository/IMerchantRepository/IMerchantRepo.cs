using Domain.Entities;
using Domain.Models.Base;
using Domain.Utils.ResultUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepository.IMerchantRepository
{
    public interface IMerchantRepo
    {
        Task<ResultRepo<object>> AddAsync(Merchant merchant);
        Task<ResultRepo<object>> UpdateAsync(int id, Merchant merchant);
        Task<string> UpdateStatus(int id, int status);
        Task<string> DeleteAsync(int id);
        Task<IList<Merchant>> GetAllAsync();
        Task<IList<Merchant>> GetAllAsync(GeneralFilterDto generalFilter);
        Task<IReadOnlyList<Merchant>> GetAllAsync(int pageNumber, int pageSize, object _oDataOptions);
        Task<Merchant> GetByIdAsync(int id);
        Task<long> GetCountAsync(object _oDataOptions);
    }
}
