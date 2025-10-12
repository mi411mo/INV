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
    public interface IOrderService
    {
        Task<ResultRepo<object>> AddAsync(OrderModel ordersModel);
        Task<OrderModel> AddByInvAsync(InvoiceModel invoice);
        Task<ResultRepo<object>> UpdateAsync(OrderModel ordersModel);
        Task<string> UpdateStatusAsync(int id, int status);
        Task<ResultRepo<object>> CancelOrderAsync(int id);
        Task<bool> ConfirmOrderAsync(string orderRef);
        Task<IList<OrderModel>> GetAllOrdersAsync();
        Task<IList<OrderModel>> GetAllOrdersAsync(GeneralFilterDto generalFilter);
        Task<ServiceResult<PagedListDto<OrderModel>>> GetAllAsync(PaginationFilter filter, object oDataOptions);
        Task<OrderModel> GetOrderByIdAsync(int id);
        Task<OrderModel> GetOrderByRefAsync(string orderRef);
        Task<int> GetCountAsync(GeneralFilterDto generalFilter);
    }
}
