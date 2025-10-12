using Domain.Entities;
using Domain.Models.Base;
using Domain.Utils.ResultUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepository.IOrderRepository
{
    public interface IOrderRepo
    {
        Task<ResultRepo<object>> AddAsync(OrderModel ordersModel);
        Task<ResultRepo<object>> UpdateAsync(OrderModel ordersModel);
        Task<ResultRepo<object>> CancelOrderAsync(int id);
        Task<bool> ConfirmOrderAsync(string orderRef);
        Task<string> UpdateStatus(int id, int status);
        Task<IList<OrderModel>> GetAllAsync();
        Task<IList<OrderModel>> GetAllAsync(GeneralFilterDto generalFilter);
        Task<IReadOnlyList<OrderModel>> GetAllAsync(int pageNumber, int pageSize, object _oDataOptions);
        Task<OrderModel> GetOrderByIdAsync(int id);
        Task<OrderModel> GetOrderRefAsync(string orderRef);
        Task<long> GetCountAsync(object _oDataOptions);
    }
}
