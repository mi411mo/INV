using Domain.Entities;
using Domain.Models.Base;
using Domain.Utils.ResultUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepository.IPaymentRepository
{
    public interface IPaymentRepo
    {
        Task<ResultRepo<object>> AddAsync(Payment payment);
        Task<ResultRepo<object>> UpdateAsync(int id, Payment payment);
        Task<ResultRepo<object>> CancelOrderAsync(int id);
        Task<string> UpdateStatus(int id, int status);
        Task<IList<Payment>> GetAllAsync();
        Task<IList<Payment>> GetAllAsync(GeneralFilterDto generalFilter);
        Task<IReadOnlyList<Payment>> GetAllAsync(int pageNumber, int pageSize, object _oDataOptions);
        Task<Payment> GetPaymentByIdAsync(int id);
        Task<Payment> GetPaymentByInvAsync(string invoiceNo);
        Task<Payment> GetPaymentByRefAsync(string payRef);
        Task<long> GetCountAsync(object _oDataOptions);
    }
}
