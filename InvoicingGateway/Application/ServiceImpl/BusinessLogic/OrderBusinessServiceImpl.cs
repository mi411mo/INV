using Application.DTOs;
using Application.Interfaces.IBusinessIndependentService.IServices;
using Application.Interfaces.IBusinessLogic;
using Application.IServices;
using Application.IWebServices;
using Application.Utils.ModelConverter;
using Domain.Entities;
using Domain.Models;
using Domain.Models.Base;
using Domain.Models.Orders.RequestDto;
using Domain.Models.Orders.ResponseDto;
using Domain.Utils;
using Domain.Utils.Enums;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ServiceImpl.BusinessLogic
{
    public class OrderBusinessServiceImpl : IOrderBusinessService
    {
        private readonly IOrderService orderService;
        private readonly IInvoiceService invoiceService;
        private readonly IMerchantService merchantService;
        private readonly IPaymentService paymentService;
        private readonly IPaymentGatewayWebService redirectPaymentService;
        private readonly ILogger<OrderBusinessServiceImpl> logger;
        private readonly IAPIModelConverterService modelConverterService;
        private readonly IAuditLogService<OrderModel> auditLogService;

        public OrderBusinessServiceImpl(IOrderService orderService, IInvoiceService invoiceService, IMerchantService merchantService, IPaymentService paymentService, IPaymentGatewayWebService redirectPaymentService, ILogger<OrderBusinessServiceImpl> logger, IAPIModelConverterService modelConverterService, IAuditLogService<OrderModel> auditLogService)
        {
            this.orderService = orderService;
            this.invoiceService = invoiceService;
            this.merchantService = merchantService;
            this.paymentService = paymentService;
            this.redirectPaymentService = redirectPaymentService;
            this.logger = logger;
            this.modelConverterService = modelConverterService;
            this.auditLogService = auditLogService;
        }
        public Task<RestAPIGenericResponseDTO<object>> ChangeStatusAsync(int id, int status, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<RestAPIGenericResponseDTO<OrderConfirmResponseDto>> ConfirmOrderAsync(string orderRef, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<OrderConfirmResponseDto>();

            try
            {
                var orderData = await orderService.GetOrderByRefAsync(orderRef);
                if (orderData == null)
                    return new RestAPIGenericResponseDTO<OrderConfirmResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.ORDER_REF_NOT_AVAILABE);

                if (orderData.Status == (int)OrderStatusEnum.Paid)
                    return new RestAPIGenericResponseDTO<OrderConfirmResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.ORDER_IS_ALREADY_PAID);

                bool isConfirmed = await orderService.ConfirmOrderAsync(orderRef);
                if (isConfirmed)
                {
                    var merchantInfo = await merchantService.GetByIdAsync(orderData.MerchantId);
                    var payment = await paymentService.AddByOrderRefAsync(orderData.OrderReference);
                    var redirectReq = new RedirectLinkPaymentsRequest().ToModel(orderData, merchantInfo, payment.Entity);
                    var redirectRes = await redirectPaymentService.RedirectLinkPayments(redirectReq);
                    var responseDto = new OrderConfirmResponseDto().ToResponse(orderData, redirectRes.Entity.TransactionUrl);
                    resultResponse = new RestAPIGenericResponseDTO<OrderConfirmResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, responseDto);
                }

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<OrderConfirmResponseDto>().WithSqlException(ex);
            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<OrderConfirmResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public Task<RestAPIGenericResponseDTO<object>> DeleteAsync(int id, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<RestAPIGenericResponseDTO<OrderResponseDto>> ExecuteAsync(OrderRequestDto request, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<OrderResponseDto>();

            try
            {
                logger.LogInformation("Orders Request DTO {}", request.ToString());
                var validationError = request.IsValid();
                if (!string.IsNullOrEmpty(validationError))
                {
                    return new RestAPIGenericResponseDTO<OrderResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, validationError);
                }
                var entity = await modelConverterService.ConvertToEntityModel<OrderModel>(request);
                var orderData = await orderService.AddAsync(entity);
                if (orderData.ReturnStatus == ReturnStatusEnum.Success)
                {
                    var orderResponse = new OrderResponseDto().ToResponse(entity);
                    await auditLogService.CreateActionAsync(profileInfo, entity);
                    resultResponse = new RestAPIGenericResponseDTO<OrderResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, orderResponse);
                }
                else
                    resultResponse = new RestAPIGenericResponseDTO<OrderResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, orderData.Message);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<OrderResponseDto>().WithSqlException(ex);
            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<OrderResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public async Task<RestAPIGenericResponseDTO<GetOrderResponseDto>> GetAll(GeneralFilterDto generalFilter, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<GetOrderResponseDto>();

            try
            {
                var data = await orderService.GetAllOrdersAsync(generalFilter);

                var responseData = await modelConverterService.ConvertToListResponseDto<GetOrderResponseDto, OrderModel>(data);
                resultResponse = new RestAPIGenericResponseDTO<GetOrderResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, responseData);
            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<GetOrderResponseDto>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<GetOrderResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public async Task<RestAPIGenericResponseDTO<OrderResponseDto>> GetById(int id, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<OrderResponseDto>();

            try
            {
                var data = await orderService.GetOrderByIdAsync(id);

                if (data != null)
                {
                    var orderResponse = await modelConverterService.ConvertToResponseDto<OrderResponseDto>(data);
                    resultResponse = new RestAPIGenericResponseDTO<OrderResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, orderResponse);
                }

                else
                    resultResponse = new RestAPIGenericResponseDTO<OrderResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.ORDER_ID_NOT_AVAILABE);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<OrderResponseDto>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<OrderResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public async Task<RestAPIGenericResponseDTO<GetOrderResponseDto>> GetByRef(string orderRef, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<GetOrderResponseDto>();

            try
            {
                var data = await orderService.GetOrderByRefAsync(orderRef);

                if (data != null)
                {
                    var orderResponse = await modelConverterService.ConvertToResponseDto<GetOrderResponseDto>(data);
                    resultResponse = new RestAPIGenericResponseDTO<GetOrderResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, orderResponse);
                }

                else
                    resultResponse = new RestAPIGenericResponseDTO<GetOrderResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.ORDER_ID_NOT_AVAILABE);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<GetOrderResponseDto>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<GetOrderResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public Task<RestAPIGenericResponseDTO<object>> UpdateAsync(OrderRequestDto request, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<RestAPIGenericResponseDTO<object>> UpdateStatusAsync(int id, int status, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<object>();

            try
            {
                var data = await orderService.GetOrderByIdAsync(id);
                if (data == null)
                    return new RestAPIGenericResponseDTO<object>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.ORDER_ID_NOT_AVAILABE);

                var updateStatus = await orderService.UpdateStatusAsync(id, (int)status);

                await auditLogService.UpdateStatusActionAsync(profileInfo, data.Status.ToString(), status.ToString());
                resultResponse = new RestAPIGenericResponseDTO<object>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, updateStatus);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<object>().WithSqlException(ex);
            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<object>().WithException(ex);
            }

            return resultResponse;
        }

        public async Task<RestAPIGenericResponseDTO<GetOrderResponseDto>> GetOrderById(int id, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<GetOrderResponseDto>();

            try
            {
                var data = await orderService.GetOrderByIdAsync(id);

                if (data != null)
                {
                    var orderResponse = await modelConverterService.ConvertToResponseDto<GetOrderResponseDto>(data);
                    resultResponse = new RestAPIGenericResponseDTO<GetOrderResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, orderResponse);
                }

                else
                    resultResponse = new RestAPIGenericResponseDTO<GetOrderResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.ORDER_ID_NOT_AVAILABE);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<GetOrderResponseDto>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<GetOrderResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public async Task<int> GetCountAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                return await orderService.GetCountAsync(generalFilter);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
