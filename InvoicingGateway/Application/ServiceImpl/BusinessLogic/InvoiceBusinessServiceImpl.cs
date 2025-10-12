using Application.DTOs;
using Application.Interfaces.IBusinessIndependentService.IServices;
using Application.Interfaces.IBusinessLogic;
using Application.IServices;
using Application.IWebServices;
using Application.Utils.ModelConverter;
using Domain.Entities;
using Domain.Models;
using Domain.Models.Base;
using Domain.Models.Invoices.RequestDto;
using Domain.Models.Invoices.ResponseDto;
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
    public class InvoiceBusinessServiceImpl : IInvoiceBusinessService
    {
        private readonly ILogger<InvoiceBusinessServiceImpl> logger;
        private readonly IInvoiceService invoiceService;
        private readonly IOrderService orderService;
        private readonly IInvoiceParameterValueService parameterValueService;
        private readonly IPaymentService paymentService;
        private readonly IMerchantService merchantService;
        private readonly IPaymentGatewayWebService redirectPaymentService;
        private readonly IAPIModelConverterService modelConverterService;
        private readonly IAuditLogService<InvoiceModel> auditLogService;

        public InvoiceBusinessServiceImpl(ILogger<InvoiceBusinessServiceImpl> logger, IInvoiceService invoiceService, IOrderService orderService, IPaymentService paymentService, IMerchantService merchantService,
            IPaymentGatewayWebService redirectPaymentService, IAPIModelConverterService modelConverterService, IInvoiceParameterValueService parameterValueService, IAuditLogService<InvoiceModel> auditLogService)
        {
            this.logger = logger;
            this.invoiceService = invoiceService;
            this.orderService = orderService;
            this.paymentService = paymentService;
            this.merchantService = merchantService;
            this.redirectPaymentService = redirectPaymentService;
            this.modelConverterService = modelConverterService;
            this.parameterValueService = parameterValueService;
            this.auditLogService = auditLogService;
        }

        public Task<RestAPIGenericResponseDTO<object>> ChangeStatusAsync(int id, int status, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<RestAPIGenericResponseDTO<InvoiceConfirmResponseDto>> ConfirmInvoiceAsync(string invNo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<InvoiceConfirmResponseDto>();

            try
            {
                var invData = await invoiceService.GetByInvoiceNo(invNo);

                if (invData == null)
                    return new RestAPIGenericResponseDTO<InvoiceConfirmResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.INVOICE_NOT_AVAILABE);

                if (invData.Status == (int)InvoiceStatusEnums.Paid)
                    return new RestAPIGenericResponseDTO<InvoiceConfirmResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.INVOICE_IS_ALREADY_PAID);

                var isUpdated = await invoiceService.UpdateStatusAsync(int.Parse(invData.Id.ToString()), (int)InvoiceStatusEnums.Unpaid);

                if (isUpdated)
                {
                    var merchantInfo = await merchantService.GetByIdAsync(invData.MerchantId);
                    var payment = await paymentService.AddByInvNoAsync(invData.InvoiceNumber);
                    var redirectReq = new RedirectLinkPaymentsRequest().ToModel(invData, merchantInfo, payment.Entity);
                    var redirectRes = await redirectPaymentService.RedirectLinkPayments(redirectReq);
                    var responseDto = new InvoiceConfirmResponseDto().ToResponse(invData, redirectRes.Entity.TransactionUrl, payment.Entity.PaymentReference);

                    resultResponse = new RestAPIGenericResponseDTO<InvoiceConfirmResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, responseDto);

                }

                //resultResponse = new RestAPIGenericResponseDTO<string>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, updateStatus);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<InvoiceConfirmResponseDto>().WithSqlException(ex);
            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<InvoiceConfirmResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public Task<RestAPIGenericResponseDTO<object>> DeleteAsync(int id, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<RestAPIGenericResponseDTO<InvoiceResponseDto>> ExecuteAsync(InvoiceRequestDto request, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<InvoiceResponseDto>();

            try
            {
                logger.LogInformation("Invoice Request DTO {}", request.ToString());
                var validationErr = request.IsValid();
                if (!string.IsNullOrEmpty(validationErr))
                {
                    // abort request processing & return error message
                    logger.LogInformation("Validation Error {}" + validationErr);
                    return new RestAPIGenericResponseDTO<InvoiceResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, validationErr);

                }
                var entity = await modelConverterService.ConvertToEntityModel<InvoiceModel>(request);
                // Create an order before creating invoice
                var addOrder = await orderService.AddByInvAsync(entity);
                entity.OrderId = addOrder.Id;

                // Create the invoice with the data or pre-created order
                var addInvoice = await invoiceService.AddAsync(entity);

                if (addInvoice.ReturnStatus == ReturnStatusEnum.Success)
                {                    
                    var invoiceEntity = await invoiceService.GetByInvoiceNo(entity.InvoiceNumber.ToString());
                    
                    try
                    {
                        if (request.CustomValues?.Count() > 0 && request.CustomValues.FirstOrDefault().ParameterId != 0)
                        {
                            foreach (var custVal in request.CustomValues)
                            {
                                var customValue = new InvoiceCustomValue()
                                {
                                    InvoiceId = int.Parse(invoiceEntity.Id.ToString()),
                                    ParameterId = custVal.ParameterId,
                                    ParameterValue = custVal.ParameterValue,
                                    CreatedAt = DateTime.Now
                                };
                                var invoiceValues = await parameterValueService.AddAsync(customValue);
                            }

                        }
                        var invoiceData = await modelConverterService.ConvertToResponseDto<InvoiceResponseDto>(invoiceEntity);

                        await auditLogService.CreateActionAsync(profileInfo, entity);
                        resultResponse = new RestAPIGenericResponseDTO<InvoiceResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, invoiceData);
                    }
                    //If the additional parameters in the invoice was not inserted
                    catch(Exception ex)
                    {
                        var invoiceData = await modelConverterService.ConvertToResponseDto<InvoiceResponseDto>(invoiceEntity);
                        resultResponse = new RestAPIGenericResponseDTO<InvoiceResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, invoiceData);
                    }
                                       
                }
                else
                    resultResponse = new RestAPIGenericResponseDTO<InvoiceResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, addInvoice.Message);
                         
            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<InvoiceResponseDto>().WithSqlException(ex);
            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<InvoiceResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public async Task<RestAPIGenericResponseDTO<InvoiceResponseDto>> GetAll(GeneralFilterDto generalFilter, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<InvoiceResponseDto>();

            try
            {
                var data = await invoiceService.GetAllAsync(generalFilter);

                var responseData = await modelConverterService.ConvertToListResponseDto<InvoiceResponseDto, InvoiceModel>(data);
                resultResponse = new RestAPIGenericResponseDTO<InvoiceResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, responseData);
            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<InvoiceResponseDto>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<InvoiceResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public async Task<RestAPIGenericResponseDTO<InvoiceResponseDto>> GetById(int id, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<InvoiceResponseDto>();

            try
            {
                var invData = await invoiceService.GetByIdAsync(id);

                if (invData != null)
                {
                    var invoiceResponse = await modelConverterService.ConvertToResponseDto<InvoiceResponseDto>(invData);
                    resultResponse = new RestAPIGenericResponseDTO<InvoiceResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, invoiceResponse);
                }
                else
                    resultResponse = new RestAPIGenericResponseDTO<InvoiceResponseDto>().WithSuccess(Constants.VALIDATION_ERROR_CODE, Constants.INVOICE_NOT_AVAILABE);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<InvoiceResponseDto>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<InvoiceResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public async Task<RestAPIGenericResponseDTO<InvoiceResponseDto>> GetByRef(string invoiceNumber, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<InvoiceResponseDto>();

            try
            {
                var invData = await invoiceService.GetByInvoiceNo(invoiceNumber);

                if (invData != null)
                {
                    var invoiceResponse = await modelConverterService.ConvertToResponseDto<InvoiceResponseDto>(invData);
                    resultResponse = new RestAPIGenericResponseDTO<InvoiceResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, invoiceResponse);
                }
                else
                    resultResponse = new RestAPIGenericResponseDTO<InvoiceResponseDto>().WithSuccess(Constants.VALIDATION_ERROR_CODE, Constants.INVOICE_NOT_AVAILABE);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<InvoiceResponseDto>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<InvoiceResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public async Task<int> GetCountAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                return  await invoiceService.GetCountAsync(generalFilter);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public Task<RestAPIGenericResponseDTO<object>> UpdateAsync(InvoiceRequestDto request, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<RestAPIGenericResponseDTO<object>> UpdateStatusAsync(int id, int status, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<object>();

            try
            {
                var data = await invoiceService.GetByIdAsync(id);
                if (data == null)
                    return new RestAPIGenericResponseDTO<object>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.INVOICE_NOT_AVAILABE);

                var updateStatus = await invoiceService.UpdateStatusAsync(id, (int)status);
                await auditLogService.UpdateStatusActionAsync(profileInfo, data.Status.ToString(), status.ToString());
                resultResponse = new RestAPIGenericResponseDTO<object>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.INVOICE_UPDATED_SUCCESSFULLY);

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
