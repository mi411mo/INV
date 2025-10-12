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
    public interface IPaymentService
    {
        Task<ResultRepo<object>> AddAsync(Payment payment);
        Task<ResultRepo<Payment>> AddByInvNoAsync(string invNo);
        Task<ResultRepo<Payment>> AddByReqPayRefAsync(string payRef);
        Task<ResultRepo<Payment>> AddByOrderRefAsync(string orderRef);
        Task<ResultRepo<object>> UpdateAsync(int id, Payment payment);
        Task<string> UpdateStatusAsync(int id, int status);
        Task<IList<Payment>> GetAllAsync();
        Task<IList<Payment>> GetAllAsync(GeneralFilterDto generalFilter);
        Task<ServiceResult<PagedListDto<Payment>>> GetAllAsync(PaginationFilter filter, object oDataOptions);
        Task<Payment> GetPaymentByIdAsync(int id);
        Task<Payment> GetPaymentByRefAsync(string payRef);
        Task<Payment> GetPaymentByInvNoAsync(string invNo);
        Task<int> GetCountAsync(GeneralFilterDto generalFilter);

    }
}
