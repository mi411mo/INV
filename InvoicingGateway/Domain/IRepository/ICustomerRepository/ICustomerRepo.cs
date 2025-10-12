using Domain.Entities;
using Domain.Models.Base;
using Domain.Utils.ResultUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepository.ICustomerRepository
{
    public interface ICustomerRepo
    {
        Task<ResultRepo<object>> AddAsync(Customer customer);
        Task<ResultRepo<object>> UpdateAsync(int id, Customer customer);
        Task<string> UpdateStatus(int id, int status);
        Task<string> DeleteAsync(int id);
        Task<IList<Customer>> GetAllAsync();
        Task<IList<Customer>> GetAllAsync(GeneralFilterDto generalFilter);
        Task<IReadOnlyList<Customer>> GetAllAsync(int pageNumber, int pageSize, object _oDataOptions);
        Task<Customer> GetByIdAsync(int id);
        Task<long> GetCountAsync(object _oDataOptions);
    }
}
