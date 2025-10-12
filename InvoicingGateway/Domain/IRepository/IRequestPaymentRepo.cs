using Domain.Entities;
using Domain.Models.Base;
using Domain.Utils.ResultUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepository
{
    public interface IRequestPaymentRepo
    {
        Task<ResultRepo<object>> AddAsync(RequestPayment payModel);
        Task<ResultRepo<object>> UpdateAsync(int id, RequestPayment payModel);
        Task<bool> UpdateStatus(int id, int status);
        Task<IList<RequestPayment>> GetAllAsync();
        Task<IList<RequestPayment>> GetAllAsync(GeneralFilterDto generalFilter);
        Task<RequestPayment> GetByIdAsync(int id);
        Task<List<RequestPayment>> GetAllAsync(int pageNumber, int pageSize, object _oDataOptions);
        Task<RequestPayment> GetByPayRefAsync(string payRef);
        Task<long> GetCountAsync(object _oDataOptions);
    }
}
