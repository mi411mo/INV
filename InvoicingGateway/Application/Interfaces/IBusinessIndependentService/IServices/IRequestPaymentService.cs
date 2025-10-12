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
    public interface IRequestPaymentService
    {
        Task<ResultRepo<object>> AddAsync(RequestPayment requestPay);
        Task<ResultRepo<object>> UpdateAsync(int id, RequestPayment requestPay);
        Task<bool> UpdateStatusAsync(int id, int status);
        Task<IList<RequestPayment>> GetAllAsync();
        Task<IList<RequestPayment>> GetAllAsync(GeneralFilterDto generalFilter);
        Task<RequestPayment> GetByIdAsync(int id);
        Task<ServiceResult<PagedListDto<RequestPayment>>> GetAllAsync(PaginationFilter filter, object oDataOptions);
        Task<RequestPayment> GetByPayRef(string payRef);
        Task<int> GetCountAsync(GeneralFilterDto generalFilter);
    }
}
