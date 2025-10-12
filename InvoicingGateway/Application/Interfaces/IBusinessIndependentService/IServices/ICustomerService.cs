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
    public interface ICustomerService
    {
        Task<ResultRepo<object>> AddAsync(Customer customer);
        Task<ResultRepo<object>> UpdateAsync(int id, Customer customer);
        Task<string> UpdateStatusAsync(int id, int status);
        Task<string> DeleteAsync(int id);
        Task<IList<Customer>> GetAllAsync();
        Task<IList<Customer>> GetAllAsync(GeneralFilterDto generalFilter);
        Task<ServiceResult<PagedListDto<Customer>>> GetAllAsync(PaginationFilter filter, object oDataOptions);
        Task<Customer> GetByIdAsync(int id);
        Task<int> GetCountAsync(GeneralFilterDto generalFilter);
    }
}
