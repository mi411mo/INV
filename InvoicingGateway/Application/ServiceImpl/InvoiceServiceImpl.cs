using Application.IServices;
using Application.Utils;
using Domain.Entities;
using Domain.IRepository.IInvoiceRepository;
using Domain.IRepository.IMerchantRepository;
using Domain.IRepository.IOrderRepository;
using Domain.Models;
using Domain.Models.Base;
using Domain.Models.Invoices.ResponseDto;
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
    public class InvoiceServiceImpl : IInvoiceService
    {
        private readonly IInvoiceRepo invoiceRepo;
        private readonly IOrderRepo orderRepo; 
        private readonly IMerchantRepo merchantRepo; 
        public InvoiceServiceImpl(IInvoiceRepo invoiceRepo, IOrderRepo orderRepo, IMerchantRepo merchantRepo)
        {
            this.invoiceRepo = invoiceRepo;
            this.orderRepo = orderRepo;
            this.merchantRepo = merchantRepo;
        }
        public async Task<ResultRepo<object>> AddAsync(InvoiceModel invoiceModel)
        {
            ResultRepo<object> res = new ResultRepo<object>();
            try
            {
                var merchants = await merchantRepo.GetAllAsync();

                if (!merchants.Any(x => x.Id == invoiceModel.MerchantId))
                {
                    return new ResultRepo<object>().setReturnResponse(ReturnStatusEnum.Failed, Constants.MERCHANT_NOT_AVAILABE);
                }
                if (merchants.Any(x => x.Id == invoiceModel.MerchantId && x.IsActive == false))
                {
                    return new ResultRepo<object>().setReturnResponse(ReturnStatusEnum.Failed, Constants.MERCHANT_NOT_ACTIVE);
                }

                res = await invoiceRepo.AddAsync(invoiceModel);

            }
            catch (Exception e)
            {
                throw;
            }
            return res;
        }


        public async Task<ResultRepo<InvoiceModel>> AddPaidInvByOrderRef(string orderRef, string payMethod)
        {
            try
            {
                var orderModel = await orderRepo.GetOrderRefAsync(orderRef);
                var invoiceModel = new InvoiceModel()
                {
                    MerchantId = orderModel.MerchantId,
                    InvoiceNumber = InvoiceUtility.GeneratingInvoiceNo(),
                    PaymentToken = InvoiceUtility.GeneratePayToken(),
                    OrderId = orderModel.Id,
                    OrderReference = orderModel.OrderReference,
                    Customer = orderModel.CustomerInfo,
                    Products = orderModel.Products,
                    TotalAmountDue = orderModel.TotalAmount,
                    AmountShipping = 0.0M,
                    AmountPaid = orderModel.TotalAmount,
                    AmountRemaining = 0.0M,
                    CurrencyCode = orderModel.CurrencyCode,
                    PaymentMethods = payMethod,
                    Status = (int)InvoiceStatusEnums.Paid,
                    Description = orderModel.Description ?? "",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                    
                };

               var isSaved = await invoiceRepo.AddAsync(invoiceModel);
                return  new ResultRepo<InvoiceModel>().setEntity(invoiceModel);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<IList<InvoiceModel>> GetAllAsync()
        {
            try
            {
                var invLst = await invoiceRepo.GetAllAsync();
                return invLst;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<ServiceResult<PagedListDto<InvoiceModel>>> GetAllAsync(PaginationFilter filter, object oDataOptions)
        {
            try
            {
                var result = await invoiceRepo.GetAllAsync(filter.PageNumber, filter.PageSize, oDataOptions);

                var res = PagedListDto<InvoiceModel>.ToPagedList(result, await invoiceRepo.GetCountAsync(oDataOptions), filter.PageNumber, filter.PageSize);
                return ServiceResult.Success(res);
            }
            catch (Exception)
            {
                return ServiceResult.Failed<PagedListDto<InvoiceModel>>(ServiceError.ServiceProvider);
            }
        }

        public async Task<IList<InvoiceModel>> GetAllAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                var lst = await invoiceRepo.GetAllAsync(generalFilter);
                return lst.Skip((generalFilter.PageNumber - 1) * generalFilter.PageSize)
                .Take(generalFilter.PageSize)
                .ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<InvoiceModel> GetByIdAsync(int id)
        {
            try
            {
                var invoiceData = await invoiceRepo.GetByIdAsync(id);
                return invoiceData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<InvoiceModel> GetByInvoiceNo(string invNo)
        {
            try
            {
                var invoiceData = await invoiceRepo.GetByInvNoAsync(invNo);
                return invoiceData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<InvoiceModel> GetByTokenAsync(long payToken)
        {
            try
            {
                var invoiceData = await invoiceRepo.GetByTokenAsync(payToken);
                return invoiceData;
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
                var lst = await invoiceRepo.GetAllAsync(generalFilter);
                return lst.Count();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ResultRepo<object>> UpdateAsync(int id, InvoiceModel invoiceModel)
        {
            ResultRepo<object> res = new ResultRepo<object>();
            try
            {
                res = await invoiceRepo.UpdateAsync(id, invoiceModel);
            }
            catch (Exception e)
            {
                throw;
            }
            return res;
        }

        public async Task<bool> UpdateStatusAsync(int id, int status)
        {
            try
            {
                var isUpdated = await invoiceRepo.UpdateStatus(id, status);
                return isUpdated;
            }
            catch (Exception ex)
            {
                throw;
            }


        }
    }
}
