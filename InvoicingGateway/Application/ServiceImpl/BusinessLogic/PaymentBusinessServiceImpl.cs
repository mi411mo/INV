using Application.DTOs;
using Application.IBusinessIndependentService;
using Application.Interfaces.IBusinessIndependentService.IServices;
using Application.Interfaces.IBusinessLogic;
using Application.IServices;
using Application.IWebServices;
using Application.Utils.ModelConverter;
using AutoMapper;
using Domain.Entities;
using Domain.Models;
using Domain.Models.Base;
using Domain.Models.Payments.RequestDto;
using Domain.Models.Payments.ResponseDto;
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
    public class PaymentBusinessServiceImpl : IPaymentBusinessService
    {
        private readonly ILogger<PaymentBusinessServiceImpl> logger;
        private readonly IPaymentService paymentService;
        private readonly IInvoiceService invoiceService;
        private readonly IOrderService orderService;
        private readonly IMerchantService merchantService;
        private readonly IRequestPaymentService reqPayService;
        private readonly IPaymentGatewayWebService gatewayWebService;
        protected readonly ICommunicationWithAccountingSystem communicationAccSys;
        private readonly IAPIModelConverterService modelConverterService;
        private readonly IAuditLogService<Payment> auditLogService;
        //private readonly IMapper mapper;

        public PaymentBusinessServiceImpl(IPaymentService paymentService, ILogger<PaymentBusinessServiceImpl> logger, ICommunicationWithAccountingSystem communicationAccSys, IRequestPaymentService reqPayService,
                                                    IAPIModelConverterService modelConverterService, IInvoiceService invoiceService, IOrderService orderService, IMerchantService merchantService, IPaymentGatewayWebService gatewayWebService, IAuditLogService<Payment> auditLogService)
        {
            this.logger = logger;
            this.modelConverterService = modelConverterService;
            this.paymentService = paymentService;
            this.invoiceService = invoiceService;
            this.orderService = orderService;
            this.communicationAccSys = communicationAccSys;
            this.merchantService = merchantService;
            this.reqPayService = reqPayService;
            this.gatewayWebService = gatewayWebService;
            this.auditLogService = auditLogService;
        }
        public Task<RestAPIGenericResponseDTO<object>> ChangeStatusAsync(int id, int status, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<RestAPIGenericResponseDTO<ConfirmPaymentResponseDto>> ConfirmPaymentAsync(string payReference, ConfirmPaymentRequestDto request, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<ConfirmPaymentResponseDto>();

            try
            {

                var paymentData = await paymentService.GetPaymentByRefAsync(payReference);
                if (paymentData == null)
                    return new RestAPIGenericResponseDTO<ConfirmPaymentResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.PAYMENT_ID_NOT_AVAILABE);

                if (request.TransactionStatus == TransactionStatusEnum.Success)
                {
                    try
                    {
                        string[] payMethods = { request.PaymentMethod };
                        var paymentMethods = JSONHelper<string[]>.GetJSONStr(payMethods) ?? "";

                        // If the payment is a request payment
                        if (!string.IsNullOrEmpty(paymentData.RequestPaymentReference))
                        {
                            var reqpaymentData = await reqPayService.GetByPayRef(paymentData.RequestPaymentReference);

                            //Deposit the paid amount to the Payment Gateway Account
                            //var requestId = Guid.NewGuid().ToString();
                            var reservationResult = await communicationAccSys.CreateDepositRequestAsync(profileInfo.ProfileId.Trim(), request.Amount, request.Currency, "1001", "101", request.TransactionReference, "101", "80000", DateTime.Now.ToString(), request.RequestId, profileInfo.ProfileId, profileInfo.ClientId, ServiceTypeEnum.Payment.ToString());
                            logger.LogInformation("[Request Id={}] Account Reservation Response: Success={} , ResponseCode={}, Message={}, ProfileId={} , Amount={}, Currency={}", request.RequestId, reservationResult.Success, reservationResult.responseCode, reservationResult.message, profileInfo.ProfileId, request.Amount, request.Currency);
                            var verifyDeposit = await communicationAccSys.DepositAcknowledgeCompletionAsync(profileInfo.ProfileId.Trim(), request.Amount, request.Currency, "1", request.TransactionReference, request.RequestId);
                            logger.LogInformation("[Request Id={}] Account Acknowledgement Response: Success={} , ResponseCode={}, Message={}, ProfileId={} , Amount={}, Currency={}", request.RequestId, verifyDeposit.Success, verifyDeposit.responseCode, verifyDeposit.message, profileInfo.ProfileId, request.Amount, request.Currency);


                            //Deposit the paid amount to the merchant account
                            var merchantEntity = await merchantService.GetByIdAsync(reqpaymentData.MerchantId);
                            reservationResult = await communicationAccSys.CreateDepositRequestAsync(merchantEntity.ProfileId.Trim(), request.Amount, request.Currency, "1001", "101", request.TransactionReference, "101", "80000", DateTime.Now.ToString(), request.RequestId, merchantEntity.ProfileId, profileInfo.ClientId, ServiceTypeEnum.Payment.ToString());
                            logger.LogInformation("[Request Id={}] Account Reservation Response: Success={} , ResponseCode={}, Message={}, ProfileId={} , Amount={}, Currency={}", request.RequestId, reservationResult.Success, reservationResult.responseCode, reservationResult.message, merchantEntity.ProfileId, request.Amount, request.Currency);
                            verifyDeposit = await communicationAccSys.DepositAcknowledgeCompletionAsync(merchantEntity.ProfileId.Trim(), request.Amount, request.Currency, "1", request.TransactionReference, request.RequestId);
                            logger.LogInformation("[Request Id={}] Account Acknowledgement Response: Success={} , ResponseCode={}, Message={}, ProfileId={} , Amount={}, Currency={}", request.RequestId, verifyDeposit.Success, verifyDeposit.responseCode, verifyDeposit.message, merchantEntity.ProfileId, request.Amount, request.Currency);

                            // update the status of the request payment 
                            await reqPayService.UpdateStatusAsync(int.Parse(reqpaymentData.Id.ToString()), (int)InvoiceStatusEnums.Paid);

                        }
                        // if the payment created by an order
                        else if (string.IsNullOrEmpty(paymentData.InvoiceReference))
                        {
                            //Deposit the paid amount to the Payment Gateway Account
                            //var requestId = Guid.NewGuid().ToString();
                            var reservationResult = await communicationAccSys.CreateDepositRequestAsync(profileInfo.ProfileId.Trim(), request.Amount, request.Currency, "1001", "101", request.TransactionReference, "101", "80000", DateTime.Now.ToString(), request.RequestId, profileInfo.ProfileId, profileInfo.ClientId, ServiceTypeEnum.Payment.ToString());
                            logger.LogInformation("[Request Id={}] Account Reservation Response: Success={} , ResponseCode={}, Message={}, ProfileId={} , Amount={}, Currency={}", request.RequestId, reservationResult.Success, reservationResult.responseCode, reservationResult.message, profileInfo.ProfileId, request.Amount, request.Currency);
                            var verifyDeposit = await communicationAccSys.DepositAcknowledgeCompletionAsync(profileInfo.ProfileId.Trim(), request.Amount, request.Currency, "1", request.TransactionReference, request.RequestId);
                            logger.LogInformation("[Request Id={}] Account Acknowledgement Response: Success={} , ResponseCode={}, Message={}, ProfileId={} , Amount={}, Currency={}", request.RequestId, verifyDeposit.Success, verifyDeposit.responseCode, verifyDeposit.message, profileInfo.ProfileId, request.Amount, request.Currency);

                            // Add paid Invoice
                            var addedInvoice = await invoiceService.AddPaidInvByOrderRef(paymentData.OrderReference, paymentMethods);
                            if (addedInvoice.Entity.OrderId > 0)
                            {
                                var isCompleted = await orderService.UpdateStatusAsync(int.Parse(addedInvoice.Entity.OrderId.ToString()), (int)OrderStatusEnum.Paid);
                            }

                            //Deposit the paid amount to the merchant account
                            var merchantEntity = await merchantService.GetByIdAsync(addedInvoice.Entity.MerchantId);
                            reservationResult = await communicationAccSys.CreateDepositRequestAsync(merchantEntity.ProfileId.Trim(), request.Amount, request.Currency, "1001", "101", request.TransactionReference, "101", "80000", DateTime.Now.ToString(), request.RequestId, merchantEntity.ProfileId, profileInfo.ClientId, ServiceTypeEnum.Payment.ToString());
                            logger.LogInformation("[Request Id={}] Account Reservation Response: Success={} , ResponseCode={}, Message={}, ProfileId={} , Amount={}, Currency={}", request.RequestId, reservationResult.Success, reservationResult.responseCode, reservationResult.message, merchantEntity.ProfileId, request.Amount, request.Currency);
                            verifyDeposit = await communicationAccSys.DepositAcknowledgeCompletionAsync(merchantEntity.ProfileId.Trim(), request.Amount, request.Currency, "1", request.TransactionReference, request.RequestId);
                            logger.LogInformation("[Request Id={}] Account Acknowledgement Response: Success={} , ResponseCode={}, Message={}, ProfileId={} , Amount={}, Currency={}", request.RequestId, verifyDeposit.Success, verifyDeposit.responseCode, verifyDeposit.message, merchantEntity.ProfileId, request.Amount, request.Currency);


                        }
                        else
                        {

                            //Deposit the paid amount to the Payment Gateway Account
                            //var requestId = Guid.NewGuid().ToString();
                            var reservationResult = await communicationAccSys.CreateDepositRequestAsync(profileInfo.ProfileId.Trim(), request.Amount, request.Currency, "1001", "101", request.TransactionReference, "101", "80000", DateTime.Now.ToString(), request.RequestId, profileInfo.ProfileId, profileInfo.ClientId, ServiceTypeEnum.Payment.ToString());
                            logger.LogInformation("[Request Id={}] Account Reservation Response: Success={} , ResponseCode={}, Message={}, ProfileId={} , Amount={}, Currency={}", request.RequestId, reservationResult.Success, reservationResult.responseCode, reservationResult.message, profileInfo.ProfileId, request.Amount, request.Currency);
                            var verifyDeposit = await communicationAccSys.DepositAcknowledgeCompletionAsync(profileInfo.ProfileId.Trim(), request.Amount, request.Currency, "1", request.TransactionReference, request.RequestId);
                            logger.LogInformation("[Request Id={}] Account Acknowledgement Response: Success={} , ResponseCode={}, Message={}, ProfileId={} , Amount={}, Currency={}", request.RequestId, verifyDeposit.Success, verifyDeposit.responseCode, verifyDeposit.message, profileInfo.ProfileId, request.Amount, request.Currency);


                            var invoiceInfo = await invoiceService.GetByInvoiceNo(paymentData.InvoiceReference);
                            InvoiceModel invoice = new()
                            {
                                AmountPaid = request.Amount,
                                AmountRemaining = invoiceInfo.AmountRemaining - request.Amount,
                                PaymentMethods = paymentMethods,
                                Status = (int)InvoiceStatusEnums.Paid
                            };

                            // Update the paidAmount, remainingAmount and status of the invoice
                            var updatePaidAmount = await invoiceService.UpdateAsync(int.Parse(invoiceInfo.Id.ToString()), invoice);
                            // Update the status of order into paid 
                            var updateOrder = await orderService.UpdateStatusAsync(int.Parse(invoiceInfo.OrderId.ToString()), (int)OrderStatusEnum.Paid);

                            //Deposit the paid amount to the merchant account
                            var merchantEntity = await merchantService.GetByIdAsync(invoiceInfo.MerchantId);
                            reservationResult = await communicationAccSys.CreateDepositRequestAsync(merchantEntity.ProfileId.Trim(), request.Amount, request.Currency, "1001", "101", request.TransactionReference, "101", "80000", DateTime.Now.ToString(), request.RequestId, merchantEntity.ProfileId, profileInfo.ClientId, ServiceTypeEnum.Payment.ToString());
                            logger.LogInformation("[Request Id={}] Account Reservation Response: Success={} , ResponseCode={}, Message={}, ProfileId={} , Amount={}, Currency={}", request.RequestId, reservationResult.Success, reservationResult.responseCode, reservationResult.message, merchantEntity.ProfileId, request.Amount, request.Currency);
                            verifyDeposit = await communicationAccSys.DepositAcknowledgeCompletionAsync(merchantEntity.ProfileId.Trim(), request.Amount, request.Currency, "1", request.TransactionReference, request.RequestId);
                            logger.LogInformation("[Request Id={}] Account Acknowledgement Response: Success={} , ResponseCode={}, Message={}, ProfileId={} , Amount={}, Currency={}", request.RequestId, verifyDeposit.Success, verifyDeposit.responseCode, verifyDeposit.message, merchantEntity.ProfileId, request.Amount, request.Currency);

                        }

                        // Update the payment status 
                        Payment payment = new()
                        {
                            Id = paymentData.Id,
                            TargetReference = request.TargetReference,
                            TransactionReference = request.TransactionReference,
                            PaymentMethod = request.PaymentMethod,
                            PaymentStatus = PaymentStatusEnum.Success,
                            PayDate = DateTime.Now
                        };


                        // Update the status, paymentMethod and payment date of the payment
                        var updateStatus = await paymentService.UpdateAsync(int.Parse(paymentData.Id.ToString()), payment);

                        resultResponse = new RestAPIGenericResponseDTO<ConfirmPaymentResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message);
                        string customerStatement = "إيداع مبلغ " + request.Amount + " إلى حسابكم مقابل سداد مرجع الطلب " + paymentData.PaymentReference + "";
                        string providerStatement = "إيداع مبلغ " + request.Amount + " إلى حسابكم مقابل سداد مرجع الطلب " + paymentData.PaymentReference + "";
                        var merchantInfo = await merchantService.GetByIdAsync(paymentData.MerchantId);
                        var accTransResult = await communicationAccSys.SaveVoucherTransactionAsync(merchantInfo.ProfileId, profileInfo.ProfileId, request.Amount, request.Currency, "منصة فاتورة", "سداد فاتورة",
                                         request.Amount, "Deposit", "CREDIT", customerStatement, request.Amount, "Deposit", "CREDIT", providerStatement, payReference, request.TargetReference,
                                        request.TransactionReference, "1", request.RequestId, ServiceTypeEnum.Payment.ToString(), merchantInfo.ProfileId, merchantInfo.ProfileId, "0", request.Currency, "",
                                        "0", request.Currency, "", "0", request.Currency, "", merchantInfo.ProfileId, "", "", merchantInfo.Phone);
                        logger.LogInformation("[Request Id={}] Saving voucherTransaction Response: Succcess={}, ResponseCode={}, Message={}, SourceProfileId={}, TargetProfileId={}", request.RequestId, accTransResult.Success, accTransResult.responseCode, accTransResult.message, merchantInfo.ProfileId, profileInfo.ProfileId);
                    }
                    catch(Exception ex)
                    {
                        logger.LogError("Throw an exception while confirming Payment: reason {}", ex.Message);                       
                        var updateStatus = await paymentService.UpdateStatusAsync(int.Parse(paymentData.Id.ToString()), (int)PaymentStatusEnum.Pending);
                        resultResponse = new RestAPIGenericResponseDTO<ConfirmPaymentResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message);
                    }                    
                }
                else
                {
                    // If the payment is failed, set the PaymentStatus into Failure
                    var updateStatus = await paymentService.UpdateStatusAsync(int.Parse(paymentData.Id.ToString()), (int)PaymentStatusEnum.Failure);
                    resultResponse = new RestAPIGenericResponseDTO<ConfirmPaymentResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message);
                }

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<ConfirmPaymentResponseDto>().WithSqlException(ex);
            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<ConfirmPaymentResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public Task<RestAPIGenericResponseDTO<object>> DeleteAsync(int id, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<RestAPIGenericResponseDTO<PaymentResponseDto>> ExecuteAsync(PaymentRequestDto request, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<PaymentResponseDto>();

            try
            {
                logger.LogInformation("Payment Request DTO {}", request.ToString());
                var validationError = request.IsValid();
                if (!string.IsNullOrEmpty(validationError))
                {
                    return new RestAPIGenericResponseDTO<PaymentResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, validationError);
                }
                var entity = await modelConverterService.ConvertToEntityModel<Payment>(request);
                var paymentData = await paymentService.AddAsync(entity);
                if (paymentData.ReturnStatus == ReturnStatusEnum.Success)
                {
                    var paymentResponse = new PaymentResponseDto().ToResponse(entity);
                    await auditLogService.CreateActionAsync(profileInfo, entity);
                    resultResponse = new RestAPIGenericResponseDTO<PaymentResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, paymentResponse);
                }
                else
                    resultResponse = new RestAPIGenericResponseDTO<PaymentResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, paymentData.Message);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<PaymentResponseDto>().WithSqlException(ex);
            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<PaymentResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public async Task<RestAPIGenericResponseDTO<PaymentResponseDto>> GetAll(GeneralFilterDto generalFilter, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<PaymentResponseDto>();

            try
            {
                var data = await paymentService.GetAllAsync(generalFilter);
                logger.LogInformation("Succeed taking data");
                var responseData = await modelConverterService.ConvertToListResponseDto<PaymentResponseDto, Payment>(data);
                resultResponse = new RestAPIGenericResponseDTO<PaymentResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, responseData);
            }
            catch (SqlException ex)
            {
                logger.LogInformation("Error due to {}", ex.Message);
                resultResponse = new RestAPIGenericResponseDTO<PaymentResponseDto>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                logger.LogInformation("Error due to {}", ex.Message);
                resultResponse = new RestAPIGenericResponseDTO<PaymentResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public Task<RestAPIGenericResponseDTO<PaymentResponseDto>> GetById(int id, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<RestAPIGenericResponseDTO<PaymentResponseDto>> GetByRef(string paymentRef, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<PaymentResponseDto>();

            try
            {
                var invData = await paymentService.GetPaymentByInvNoAsync(paymentRef);

                if (invData != null)
                {
                    var invoiceResponse = await modelConverterService.ConvertToResponseDto<PaymentResponseDto>(invData);
                    resultResponse = new RestAPIGenericResponseDTO<PaymentResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, invoiceResponse);
                }
                else
                    resultResponse = new RestAPIGenericResponseDTO<PaymentResponseDto>().WithSuccess(Constants.VALIDATION_ERROR_CODE, Constants.INVOICE_NOT_AVAILABE);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<PaymentResponseDto>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<PaymentResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public async Task<int> GetCountAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                return await paymentService.GetCountAsync(generalFilter);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<RestAPIGenericResponseDTO<PaymentResponseDto>> GetPaymentById(int id, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<PaymentResponseDto>();

            try
            {
                var data = await paymentService.GetPaymentByIdAsync(id);

                if (data != null)
                {
                    var paymentResponse = await modelConverterService.ConvertToResponseDto<PaymentResponseDto>(data);
                    resultResponse = new RestAPIGenericResponseDTO<PaymentResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, paymentResponse);
                }                   
                else
                    resultResponse = new RestAPIGenericResponseDTO<PaymentResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.PAYMENT_ID_NOT_AVAILABE);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<PaymentResponseDto>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<PaymentResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public Task<RestAPIGenericResponseDTO<object>> UpdateAsync(PaymentRequestDto request, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<RestAPIGenericResponseDTO<object>> UpdateStatusAsync(int id, int status, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<object>();

            try
            {
                var data = await paymentService.GetPaymentByIdAsync(id);
                if (data == null)
                    return new RestAPIGenericResponseDTO<object>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.PAYMENT_ID_NOT_AVAILABE);

                var updateStatus = await paymentService.UpdateStatusAsync(id, (int)status);

                await auditLogService.UpdateStatusActionAsync(profileInfo, data.PaymentStatus.ToString(), status.ToString());
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

        public async Task<RestAPIGenericResponseDTO<PaymentMethodsResponseDto>> GetPaymentMethods(GeneralFilterDto generalFilter, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<PaymentMethodsResponseDto>();

            try
            {
                var data = await gatewayWebService.GetPaymentMethods();
                if(data.Success)
                {
                    var responseData = await modelConverterService.ConvertToListResponseDto<PaymentMethodsResponseDto, PaymentMethod>(data.Entities);
                    return new RestAPIGenericResponseDTO<PaymentMethodsResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, responseData);
                }
                resultResponse = new RestAPIGenericResponseDTO<PaymentMethodsResponseDto>().WithError(Constants.SERVER_ERROR_CODE, Constants.SERVER_ERROR_MSG);
            }
            catch (SqlException ex)
            {
                logger.LogInformation("Error due to {}", ex.Message);
                resultResponse = new RestAPIGenericResponseDTO<PaymentMethodsResponseDto>().WithSqlException(ex);
            }
            catch (Exception ex)
            {
                logger.LogInformation("Error due to {}", ex.Message);
                resultResponse = new RestAPIGenericResponseDTO<PaymentMethodsResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public async Task<RestAPIGenericResponseDTO<DirectPaymentResponseDto>> DirectPaymentAsync(DirectPaymentRequestDto request, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            try
            {
                
                // Get the invoice data to check availability of the invoice
                var invoiceInfo = await invoiceService.GetByInvoiceNo(request.InvoiceReference.Trim());

                if (invoiceInfo == null)
                {
                    var response = new RestAPIGenericResponseDTO<DirectPaymentResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.INVOICE_NOT_AVAILABE);                   
                    return response;
                }

                //Add initiated payment
                var inititedPayment = await paymentService.AddByInvNoAsync(request.InvoiceReference.Trim());

                if (invoiceInfo.Status == (int)InvoiceStatusEnums.Paid)
                {
                    var response = new RestAPIGenericResponseDTO<DirectPaymentResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.INVOICE_IS_ALREADY_PAID);
                    var updateStatus = await paymentService.UpdateStatusAsync(int.Parse(inititedPayment.Entity.Id.ToString()), (int)PaymentStatusEnum.Failure);
                    return response;
                }

                // If the paid amount is less than the invoice amount
                if (invoiceInfo.TotalAmountDue > request.Amount)
                {
                    var response = new RestAPIGenericResponseDTO<DirectPaymentResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.INSUFFIENT_LESS_AMOUNT_Paid);
                    var updateStatus = await paymentService.UpdateStatusAsync(int.Parse(inititedPayment.Entity.Id.ToString()), (int)PaymentStatusEnum.Failure);
                    return response;
                }
                
                // If the paid amount is less than the invoice amount
                if (invoiceInfo.TotalAmountDue < request.Amount)
                {
                    var response = new RestAPIGenericResponseDTO<DirectPaymentResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.INSUFFIENT_GREATER_AMOUNT_Paid);
                    var updateStatus = await paymentService.UpdateStatusAsync(int.Parse(inititedPayment.Entity.Id.ToString()), (int)PaymentStatusEnum.Failure);
                    return response;
                }

                try
                {
                    string[] payMethods = { request.PaymentMethod };
                    var paymentMethod = JSONHelper<string[]>.GetJSONStr(payMethods) ?? "";
                    //Deposit the paid amount to the Payment Gateway Account
                    //var requestId = Guid.NewGuid().ToString();
                    var reservationResult = await communicationAccSys.CreateDepositRequestAsync(profileInfo.ProfileId.Trim(), request.Amount, request.CurrencyCode, "1001", "101", request.TransactionReference, "101", "80000", DateTime.Now.ToString(), request.RequestId, profileInfo.ProfileId, profileInfo.ClientId, ServiceTypeEnum.Payment.ToString());
                    logger.LogInformation("[Request Id={}] Account Reservation Response: Success={} , ResponseCode={}, Message={}, ProfileId={} , Amount={}, Currency={}", request.RequestId, reservationResult.Success, reservationResult.responseCode, reservationResult.message, profileInfo.ProfileId, request.Amount, request.CurrencyCode);

                    var verifyDeposit = await communicationAccSys.DepositAcknowledgeCompletionAsync(profileInfo.ProfileId.Trim(), request.Amount, request.CurrencyCode, "1", request.TransactionReference, request.RequestId);
                    logger.LogInformation("[Request Id={}] Account Acknowledgement Response: Success={} , ResponseCode={}, Message={}, ProfileId={} , Amount={}, Currency={}", request.RequestId, verifyDeposit.Success, verifyDeposit.responseCode, verifyDeposit.message, profileInfo.ProfileId, request.Amount, request.CurrencyCode);


                    InvoiceModel invoice = new()
                    {
                        AmountPaid = request.Amount,
                        AmountRemaining = invoiceInfo.AmountRemaining - request.Amount,
                        PaymentMethods = paymentMethod,
                        Status = (int)InvoiceStatusEnums.Paid
                    };

                    // Update the paidAmount, remainingAmount and status of the invoice
                    var updatePaidAmount = await invoiceService.UpdateAsync(int.Parse(invoiceInfo.Id.ToString()), invoice);
                    // Update the status of order into paid 
                    var updateOrder = await orderService.UpdateStatusAsync(int.Parse(invoiceInfo.OrderId.ToString()), (int)OrderStatusEnum.Paid);

                    //Deposit the paid amount to the merchant account
                    var merchantEntity = await merchantService.GetByIdAsync(invoiceInfo.MerchantId);
                    reservationResult = await communicationAccSys.CreateDepositRequestAsync(merchantEntity.ProfileId.Trim(), request.Amount, request.CurrencyCode, "1001", "101", request.TransactionReference, "101", "80000", DateTime.Now.ToString(), request.RequestId, merchantEntity.ProfileId, profileInfo.ClientId, ServiceTypeEnum.Payment.ToString());
                    logger.LogInformation("[Request Id={}] Account Reservation Response: Success={} , ResponseCode={}, Message={}, ProfileId={} , Amount={}, Currency={}", request.RequestId, reservationResult.Success, reservationResult.responseCode, reservationResult.message, merchantEntity.ProfileId, request.Amount, request.CurrencyCode);

                    // TODO: To check if the amount to be deposit directly into the merchant account or not
                    verifyDeposit = await communicationAccSys.DepositAcknowledgeCompletionAsync(merchantEntity.ProfileId.Trim(), request.Amount, request.CurrencyCode, "1", request.TransactionReference, request.RequestId);
                    logger.LogInformation("[Request Id={}] Account Acknowledgement Response: Success={} , ResponseCode={}, Message={}, ProfileId={} , Amount={}, Currency={}", request.RequestId, verifyDeposit.Success, verifyDeposit.responseCode, verifyDeposit.message, merchantEntity.ProfileId, request.Amount, request.CurrencyCode);


                    // Update the payment status 
                    Payment payment = new()
                    {
                        Id = int.Parse(inititedPayment.Entity.Id.ToString()),
                        TargetReference = request.TransactionReference,
                        TransactionReference = request.TransactionReference,
                        PaymentMethod = request.PaymentMethod ?? "Easy Wallet",
                        PaymentStatus = PaymentStatusEnum.Success,
                        PayDate = DateTime.Now
                    };

                    // Update the status, paymentMethod and payment date of the payment
                    var updateStatus = await paymentService.UpdateAsync(int.Parse(inititedPayment.Entity.Id.ToString()), payment);


                    string customerStatement = "إيداع مبلغ " + request.Amount + " إلى حسابكم مقابل سداد مرجع الطلب " + inititedPayment.Entity.PaymentReference + "";
                    string providerStatement = "إيداع مبلغ " + request.Amount + " إلى حسابكم مقابل سداد مرجع الطلب " + inititedPayment.Entity.PaymentReference + "";

                    var merchantInfo = await merchantService.GetByIdAsync(inititedPayment.Entity.MerchantId);

                    var accTransResult = await communicationAccSys.SaveVoucherTransactionAsync(merchantInfo.ProfileId, profileInfo.ProfileId, request.Amount, request.CurrencyCode, "منصة فاتورة", "سداد فاتورة", request.Amount, "Deposit", "CREDIT", customerStatement, request.Amount, "Deposit", "CREDIT", 
                                                                                               providerStatement, inititedPayment.Entity.PaymentReference, request.TransactionReference, request.TransactionReference, "1", request.RequestId, ServiceTypeEnum.Payment.ToString(), merchantInfo.ProfileId, merchantInfo.ProfileId, "0", request.CurrencyCode, "",
                                                                                               "0", request.CurrencyCode, "", "0", request.CurrencyCode, "", merchantInfo.ProfileId, "", "", merchantInfo.Phone);
                    logger.LogInformation("[Request Id={}] Saving voucherTransaction Response: Succcess={}, ResponseCode={}, Message={}, SourceProfileId={}, TargetProfileId={}", request.RequestId, accTransResult.Success, accTransResult.responseCode, accTransResult.message, profileInfo.ProfileId, merchantInfo.ProfileId);


                    var responseEntity = new DirectPaymentResponseDto(request.TransactionReference, request.Amount, request.TransactionReference, TransactionStatusEnum.Success);
                    return new RestAPIGenericResponseDTO<DirectPaymentResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, responseEntity);

                }
                catch(Exception ex)
                {
                    var responseEntity = new DirectPaymentResponseDto(request.TransactionReference, request.Amount, request.TransactionReference, TransactionStatusEnum.Pending);
                    var responseDto = new RestAPIGenericResponseDTO<DirectPaymentResponseDto>().WithPending(Constants.FATORA_PENDING_CODE, Constants.FATORA_PENDING_MSG, responseEntity);
                    var updateStatus = await paymentService.UpdateStatusAsync(int.Parse(inititedPayment.Entity.Id.ToString()), (int)PaymentStatusEnum.Pending);
                    return responseDto;
                }


            }
            catch (Exception ex)
            {
                return new RestAPIGenericResponseDTO<DirectPaymentResponseDto>().WithError(Constants.SERVER_ERROR_CODE, Constants.SERVER_ERROR_MSG);              
            }
        }

        public async Task<RestAPIGenericResponseDTO<CheckProviderPaymentStatusResponseDto>> CheckProviderPaymentStatus(string paymentRef, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            try
            {
                var paymentData = await paymentService.GetPaymentByRefAsync(paymentRef);
                if (paymentData == null)
                    return new RestAPIGenericResponseDTO<CheckProviderPaymentStatusResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.PAYMENT_ID_NOT_AVAILABE);

                // To check the payment status from Sadad Gateway not from Link
                if (paymentData.RequestId is null)
                {
                    return new RestAPIGenericResponseDTO<CheckProviderPaymentStatusResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.PAYMENT_ID_NOT_AVAILABE);
                }

                var providerPaymentRes = await gatewayWebService.CheckPortalPaymentStatus(paymentData.RequestId);
                if(providerPaymentRes.entity is not null)
                {
                    var responseDto = new CheckPortalPaymentStatusResponse().ToResponse(providerPaymentRes);
                    return new RestAPIGenericResponseDTO<CheckProviderPaymentStatusResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, responseDto);
                }
                else
                {
                    return new RestAPIGenericResponseDTO<CheckProviderPaymentStatusResponseDto>().WithError(providerPaymentRes.ResponseCode, providerPaymentRes.Message);
                }
                    


            }
            catch(Exception ex)
            {
                return new RestAPIGenericResponseDTO<CheckProviderPaymentStatusResponseDto>().WithError(Constants.SERVER_ERROR_CODE, Constants.SERVER_ERROR_MSG);
            }

        }
    }
}
