using Application.DTOs;
using Application.Interfaces.IBusinessIndependentService.IServices;
using Application.Interfaces.IBusinessLogic;
using Application.IServices;
using Application.IWebServices;
using Application.Utils.ModelConverter;
using Domain.Entities;
using Domain.Models;
using Domain.Models.Base;
using Domain.Models.RequestPayments;
using Domain.Utils;
using Domain.Utils.Enums;
using Microsoft.Extensions.Logging;
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
    public class RequestPaymentBusinessServiceImpl : IRequestPaymentBusinessService
    {
        private readonly ILogger<RequestPaymentBusinessServiceImpl> logger;
        private readonly IRequestPaymentService requestPaymentService;
        private readonly IPaymentService paymentService;
        private readonly IMerchantService merchantService;
        private readonly IPaymentGatewayWebService redirectPaymentService;
        private readonly IAPIModelConverterService modelConverterService;
        private readonly IAuditLogService<RequestPayment> auditLogService;

        public RequestPaymentBusinessServiceImpl(ILogger<RequestPaymentBusinessServiceImpl> logger, IRequestPaymentService requestPaymentService, IMerchantService merchantService,
            IPaymentGatewayWebService redirectPaymentService, IAPIModelConverterService modelConverterService, IPaymentService paymentService, IAuditLogService<RequestPayment> auditLogService)
        {
            this.logger = logger;
            this.requestPaymentService = requestPaymentService;
            this.paymentService = paymentService;
            this.merchantService = merchantService;
            this.redirectPaymentService = redirectPaymentService;
            this.modelConverterService = modelConverterService;
            this.auditLogService = auditLogService;
        }
        public Task<RestAPIGenericResponseDTO<object>> ChangeStatusAsync(int id, int status, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<RestAPIGenericResponseDTO<RequestPaymentConfirmResponseDto>> ConfirmRequestPayAsync(string paymentRef, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<RequestPaymentConfirmResponseDto>();

            try
            {
                var reqPayData = await requestPaymentService.GetByPayRef(paymentRef);
                if (reqPayData == null)
                    return new RestAPIGenericResponseDTO<RequestPaymentConfirmResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.REQUEST_PAYMENT_NOT_AVAILABE);

                if (reqPayData.Status == (int)InvoiceStatusEnums.Paid)
                    return new RestAPIGenericResponseDTO<RequestPaymentConfirmResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.REQUEST_PAYMENT_IS_ALREADY_PAID);

                var isUpdated = await requestPaymentService.UpdateStatusAsync(int.Parse(reqPayData.Id.ToString()), (int)InvoiceStatusEnums.Unpaid);

                if (isUpdated)
                {
                    var merchantInfo = await merchantService.GetByIdAsync(reqPayData.MerchantId);
                    var payment = await paymentService.AddByReqPayRefAsync(reqPayData.RequestPaymentReference);
                    var redirectReq = new RedirectLinkPaymentsRequest().ToModel(reqPayData, merchantInfo, payment.Entity);
                    var redirectRes = await redirectPaymentService.RedirectLinkPayments(redirectReq);
                    var responseDto = new RequestPaymentConfirmResponseDto().ToResponse(reqPayData, redirectRes.Entity.TransactionUrl);

                    resultResponse = new RestAPIGenericResponseDTO<RequestPaymentConfirmResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, responseDto);

                }

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<RequestPaymentConfirmResponseDto>().WithSqlException(ex);
            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<RequestPaymentConfirmResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public Task<RestAPIGenericResponseDTO<object>> DeleteAsync(int id, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<RestAPIGenericResponseDTO<RequestPaymentResponseDto>> ExecuteAsync(RequestPaymentRequestDto request, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<RequestPaymentResponseDto>();

            try
            {
                logger.LogInformation("Payment Request DTO {}", request.ToString());
                var validationErr = request.IsValid();
                if (!string.IsNullOrEmpty(validationErr))
                {
                    // abort request processing & return error message
                    logger.LogInformation("Validation Error {}" + validationErr);
                    return new RestAPIGenericResponseDTO<RequestPaymentResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, validationErr);

                }
                var entity = await modelConverterService.ConvertToEntityModel<RequestPayment>(request);

                var addReqPay = await requestPaymentService.AddAsync(entity);

                if (addReqPay.ReturnStatus == ReturnStatusEnum.Success)
                {
                    var reqPayEntity = await requestPaymentService.GetByPayRef(entity.RequestPaymentReference.ToString());
                    var reqPayData = await modelConverterService.ConvertToResponseDto<RequestPaymentResponseDto>(reqPayEntity);
                    await auditLogService.CreateActionAsync(profileInfo, entity);
                    resultResponse = new RestAPIGenericResponseDTO<RequestPaymentResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, reqPayData);
                    
                }
                else
                    resultResponse = new RestAPIGenericResponseDTO<RequestPaymentResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, addReqPay.Message);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<RequestPaymentResponseDto>().WithSqlException(ex);
            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<RequestPaymentResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public async Task<RestAPIGenericResponseDTO<RequestPaymentResponseDto>> GetAll(GeneralFilterDto generalFilter, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<RequestPaymentResponseDto>();

            try
            {
                var data = await requestPaymentService.GetAllAsync(generalFilter);

                var responseData = await modelConverterService.ConvertToListResponseDto<RequestPaymentResponseDto, RequestPayment>(data);
                resultResponse = new RestAPIGenericResponseDTO<RequestPaymentResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, responseData);
            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<RequestPaymentResponseDto>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<RequestPaymentResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public async Task<RestAPIGenericResponseDTO<RequestPaymentResponseDto>> GetById(int id, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<RequestPaymentResponseDto>();

            try
            {
                var reqPayData = await requestPaymentService.GetByIdAsync(id);

                if (reqPayData != null)
                {
                    var reqPayResponse = await modelConverterService.ConvertToResponseDto<RequestPaymentResponseDto>(reqPayData);
                    resultResponse = new RestAPIGenericResponseDTO<RequestPaymentResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, reqPayResponse);
                }
                else
                    resultResponse = new RestAPIGenericResponseDTO<RequestPaymentResponseDto>().WithSuccess(Constants.VALIDATION_ERROR_CODE, Constants.REQUEST_PAYMENT_NOT_AVAILABE);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<RequestPaymentResponseDto>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<RequestPaymentResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public async Task<RestAPIGenericResponseDTO<RequestPaymentResponseDto>> GetByRef(string paymentRef, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<RequestPaymentResponseDto>();

            try
            {
                var invData = await requestPaymentService.GetByPayRef(paymentRef);

                if (invData != null)
                {
                    var invoiceResponse = await modelConverterService.ConvertToResponseDto<RequestPaymentResponseDto>(invData);
                    resultResponse = new RestAPIGenericResponseDTO<RequestPaymentResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, invoiceResponse);
                }
                else
                    resultResponse = new RestAPIGenericResponseDTO<RequestPaymentResponseDto>().WithSuccess(Constants.VALIDATION_ERROR_CODE, Constants.REQUEST_PAYMENT_NOT_AVAILABE);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<RequestPaymentResponseDto>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<RequestPaymentResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public async Task<int> GetCountAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                return await requestPaymentService.GetCountAsync(generalFilter);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public Task<RestAPIGenericResponseDTO<object>> UpdateAsync(RequestPaymentRequestDto request, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<RestAPIGenericResponseDTO<object>> UpdateStatusAsync(int id, int status, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<object>();

            try
            {
                var data = await requestPaymentService.GetByIdAsync(id);
                if (data == null)
                    return new RestAPIGenericResponseDTO<object>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.REQUEST_PAYMENT_NOT_AVAILABE);

                var updateStatus = await requestPaymentService.UpdateStatusAsync(id, (int)status);

                await auditLogService.UpdateStatusActionAsync(profileInfo, data.Status.ToString(), status.ToString());
                resultResponse = new RestAPIGenericResponseDTO<object>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.REQUEST_PAYMENT_UPDATED_SUCCESSFULLY);

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
    }
}
