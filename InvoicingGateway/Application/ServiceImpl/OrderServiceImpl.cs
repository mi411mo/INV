using Application.IServices;
using Application.Utils;
using Domain.Entities;
using Domain.IRepository.IInvoiceRepository;
using Domain.IRepository.IMerchantRepository;
using Domain.IRepository.IOrderRepository;
using Domain.Models;
using Domain.Models.Base;
using Domain.Models.Orders.RequestDto;
using Domain.Utils;
using Domain.Utils.Enums;
using Domain.Utils.ResultUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceImpl
{
    public class OrderServiceImpl : IOrderService
    {
        private readonly IOrderRepo orderRepo;
        private readonly IInvoiceRepo invoiceRepo;
        private readonly IMerchantRepo merchantRepo;
        public OrderServiceImpl(IOrderRepo orderRepo, IInvoiceRepo invoiceRepo, IMerchantRepo merchantRepo)
        {
            this.orderRepo = orderRepo;
            this.invoiceRepo = invoiceRepo;
            this.merchantRepo = merchantRepo;
        }
        public async Task<ResultRepo<object>> AddAsync(OrderModel order)
        {
            ResultRepo<object> res = new ResultRepo<object>();
            try
            {
                //var orders = await orderRepo.GetAllAsync();
                var merchants = await merchantRepo.GetAllAsync();
                
                /*if (orders.Any(x => x.OrderReference == order.OrderReference))
                {
                    return new ResultRepo<object>().setReturnResponse(ReturnStatusEnum.Failed, Constants.REFERENCE_IS_DUPLICATED);
                }*/
                if (!merchants.Any(x => x.Id == order.MerchantId))
                {
                    return new ResultRepo<object>().setReturnResponse(ReturnStatusEnum.Failed, Constants.MERCHANT_NOT_AVAILABE);
                }
                if (merchants.Any(x => x.Id == order.MerchantId && x.IsActive == false))
                {
                    return new ResultRepo<object>().setReturnResponse(ReturnStatusEnum.Failed, Constants.MERCHANT_NOT_ACTIVE);
                }

                res = await orderRepo.AddAsync(order);
            }
            catch (Exception ex)
            {
                throw;
            }
            return res;
        }

        public async Task<bool> ConfirmOrderAsync(string orderRef)
        {
            try
            {
                var isConfirmed = await orderRepo.ConfirmOrderAsync(orderRef);
                return isConfirmed;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<ResultRepo<object>> CancelOrderAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<PagedListDto<OrderModel>>> GetAllAsync(PaginationFilter filter, object oDataOptions)
        {

            try
            {
                var result = await orderRepo.GetAllAsync(filter.PageNumber, filter.PageSize, oDataOptions);
                var res = PagedListDto<OrderModel>.ToPagedList(result, await orderRepo.GetCountAsync(oDataOptions), filter.PageNumber, filter.PageSize);
                return ServiceResult.Success(res);
            }
            catch (Exception)
            {
                return ServiceResult.Failed<PagedListDto<OrderModel>>(ServiceError.ServiceProvider);
            }
        }

        public async Task<IList<OrderModel>> GetAllOrdersAsync()
        {
            try
            {
                var lst = await orderRepo.GetAllAsync();
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<OrderModel> GetOrderByIdAsync(int id)
        {
            try
            {
                var orderData = await orderRepo.GetOrderByIdAsync(id);
                return orderData;
            }          
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<OrderModel> GetOrderByRefAsync(string orderRef)
        {
            try
            {
                var orderData = await orderRepo.GetOrderRefAsync(orderRef);
                return orderData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<ResultRepo<object>> UpdateAsync(OrderModel ordersModel)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UpdateStatusAsync(int id, int status)
        {
            try
            {
                var updateStatus = await orderRepo.UpdateStatus(id, status);
                return updateStatus;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<IList<OrderModel>> GetAllOrdersAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                var lst = await orderRepo.GetAllAsync(generalFilter);
                return lst.Skip((generalFilter.PageNumber - 1) * generalFilter.PageSize)
                .Take(generalFilter.PageSize)
                .ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> GetCountAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                var lst = await orderRepo.GetAllAsync(generalFilter);
                return lst.Count();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<OrderModel> AddByInvAsync(InvoiceModel invoice)
        {
            OrderModel addedOrder = new OrderModel();
            try
            {
                var orderEnitity = new OrderModel
                {
                    MerchantId = invoice.MerchantId,
                    TotalAmount = invoice.TotalAmountDue,
                    CurrencyCode = invoice.CurrencyCode,
                    OrderReference = invoice.OrderReference,
                    Status = (int)OrderStatusEnum.Pending,
                    Description = invoice.Description ?? "",
                    Products = invoice.Products,
                    CustomerInfo = invoice.Customer,
                    CreatedAt = DateTime.Now,
                    CustomerId = invoice.CustomerId,
                    ClientId = invoice.ClientId,
                    UserId = invoice.UserId
                };

                ResultRepo<object> res = await orderRepo.AddAsync(orderEnitity);
                addedOrder = await orderRepo.GetOrderRefAsync(orderEnitity.OrderReference);
            }
            catch (Exception ex)
            {
                throw;
            }
            return addedOrder;
        }
    }
}
