using Application.IServices;
using Application.Utils;
using Domain.Entities;
using Domain.IRepository;
using Domain.IRepository.IInvoiceRepository;
using Domain.IRepository.IOrderRepository;
using Domain.IRepository.IPaymentRepository;
using Domain.Models;
using Domain.Models.Base;
using Domain.Models.Invoices.RequestDto;
using Domain.Utils;
using Domain.Utils.Enums;
using Domain.Utils.ResultUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceImpl
{
    public class PaymentServiceImpl : IPaymentService
    {
        private readonly IPaymentRepo paymentRepo;
        private readonly IInvoiceRepo invoiceRepo;
        private readonly IOrderRepo orderRepo;
        private readonly IRequestPaymentRepo requestPayRepo;
        public PaymentServiceImpl(IPaymentRepo paymentRepo, IInvoiceRepo invoiceRepo, IOrderRepo orderRepo, IRequestPaymentRepo requestPayRepo)
        {
            this.paymentRepo = paymentRepo;
            this.invoiceRepo = invoiceRepo;
            this.orderRepo = orderRepo;
            this.requestPayRepo = requestPayRepo;
        }
        public async Task<ResultRepo<object>> AddAsync(Payment payment)
        {
            ResultRepo<object> res = new ResultRepo<object>();
            try
            {               
                res = await paymentRepo.AddAsync(payment);
            }
            catch (Exception ex)
            {
                throw;
            }
            return res;
        }

        public async Task<ResultRepo<Payment>> AddByInvNoAsync(string invNo)
        {
            try
            {
                var invoiceModel = await invoiceRepo.GetByInvNoAsync(invNo);
                var payment = new Payment()
                {

                    //TransactionReference = "",
                    RequestId = Guid.NewGuid().ToString(),
                    CustomerInfo = invoiceModel.Customer ?? "",
                    PaymentReference = InvoiceUtility.GeneratePayReference(),
                    AmountDue = invoiceModel.TotalAmountDue,
                    CurrencyCode = invoiceModel.CurrencyCode,
                    MerchantId = invoiceModel.MerchantId,
                    OrderReference = invoiceModel.OrderReference,
                    InvoiceReference = invoiceModel.InvoiceNumber,
                    PaymentType = PaymentTypeEnum.Invoices,
                    //PaymentMethod = invoiceModel.PaymentMethods,
                    PaymentStatus = PaymentStatusEnum.initiated,
                    //Details = requestDto.Details,
                    IssueDate = DateTime.Now,
                    CustomerId = invoiceModel.CustomerId,
                    UserId = invoiceModel.UserId,
                    ClientId = invoiceModel.ClientId,
                    PayDate = null
                };

                var isSaved = await paymentRepo.AddAsync(payment);
                var paymentData = await paymentRepo.GetPaymentByRefAsync(payment.PaymentReference);
                return new ResultRepo<Payment>().setEntity(paymentData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<ResultRepo<Payment>> AddByReqPayRefAsync(string payRef)
        {
            try
            {
                var reqPayModel = await requestPayRepo.GetByPayRefAsync(payRef);
                var payment = new Payment()
                {
                    RequestId = Guid.NewGuid().ToString(),
                    CustomerInfo = reqPayModel.Customer ?? string.Empty,
                    PaymentReference = InvoiceUtility.GeneratePayReference(),
                    AmountDue = reqPayModel.TotalAmount,
                    CurrencyCode = reqPayModel.CurrencyCode,
                    MerchantId = reqPayModel.MerchantId,
                    PaymentType = PaymentTypeEnum.RequestPayments,
                    RequestPaymentReference = reqPayModel.RequestPaymentReference,
                    PaymentStatus = PaymentStatusEnum.initiated,
                    //Details = requestDto.Details,
                    IssueDate = DateTime.Now,
                    CustomerId = reqPayModel.CustomerId,
                    UserId = reqPayModel.UserId,
                    ClientId = reqPayModel.ClientId ?? string.Empty,
                    PayDate = null
                };

                var isSaved = await paymentRepo.AddAsync(payment);
                return new ResultRepo<Payment>().setEntity(payment);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ResultRepo<Payment>> AddByOrderRefAsync(string orderRef)
        {
            try
            {
                var orderData = await orderRepo.GetOrderRefAsync(orderRef);
                var payment = new Payment()
                {

                    //TransactionReference = "",
                    RequestId = Guid.NewGuid().ToString(),
                    CustomerInfo = orderData.CustomerInfo ?? "",
                    PaymentReference = InvoiceUtility.GeneratePayReference(),
                    AmountDue = orderData.TotalAmount,
                    CurrencyCode = orderData.CurrencyCode,
                    MerchantId = orderData.MerchantId,
                    OrderReference = orderData.OrderReference,
                    PaymentType = PaymentTypeEnum.Orders,
                    //InvoiceReference = invoiceModel.InvoiceNumber,
                    //PaymentMethod = invoiceModel.PaymentMethods,
                    PaymentStatus = PaymentStatusEnum.initiated,
                    //Details = requestDto.Details,
                    IssueDate = DateTime.Now,
                    PayDate = null
                };

                var isSaved = await paymentRepo.AddAsync(payment);
                return new ResultRepo<Payment>().setEntity(payment);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IList<Payment>> GetAllAsync()
        {
            try
            {
                var lst = await paymentRepo.GetAllAsync();
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ServiceResult<PagedListDto<Payment>>> GetAllAsync(PaginationFilter filter, object oDataOptions)
        {
            try
            {
                var result = await paymentRepo.GetAllAsync(filter.PageNumber, filter.PageSize, oDataOptions);
                var res = PagedListDto<Payment>.ToPagedList(result, await paymentRepo.GetCountAsync(oDataOptions), filter.PageNumber, filter.PageSize);
                return ServiceResult.Success(res);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failed<PagedListDto<Payment>>(ServiceError.ServiceProvider);
            }
        }

        public async Task<IList<Payment>> GetAllAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                var lst = await paymentRepo.GetAllAsync(generalFilter);
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
                var lst = await paymentRepo.GetAllAsync(generalFilter);
                return lst.Count();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Payment> GetPaymentByIdAsync(int id)
        {
            try
            {
                var paymentData = await paymentRepo.GetPaymentByIdAsync(id);
                return paymentData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Payment> GetPaymentByInvNoAsync(string invNo)
        {
            try
            {
                var paymentData = await paymentRepo.GetPaymentByInvAsync(invNo);
                return paymentData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Payment> GetPaymentByRefAsync(string payRef)
        {
            try
            {
                var paymentData = await paymentRepo.GetPaymentByRefAsync(payRef);
                return paymentData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ResultRepo<object>> UpdateAsync(int id, Payment payment)
        {
            ResultRepo<object> res = new ResultRepo<object>();
            try
            {
                res = await paymentRepo.UpdateAsync(id, payment);
            }
            catch (Exception e)
            {
                throw;
            }
            return res;
        }

        public async Task<string> UpdateStatusAsync(int id, int status)
        {
            try
            {
                var updateStatus = await paymentRepo.UpdateStatus(id, status);
                return updateStatus;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
