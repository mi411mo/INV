using Application.DTOs;
using Application.IServices;
using Application.IWebServices;
using Application.Utils.ModelConverter;
using Domain.Entities;
using Domain.Models;
using Domain.Models.Orders.RequestDto;
using Domain.Models.Orders.ResponseDto;
using Domain.Models.Payments.RequestDto;
using Domain.Models.Payments.ResponseDto;
using Domain.Utils;
using Domain.Utils.Enums;
using InvoicingGatewayAPI.Utils;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicingGatewayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly ILogger<PaymentsController> logger;
        private readonly IPaymentService paymentService;
        private readonly IInvoiceService invoiceService;
        private readonly IOrderService orderService;
        private readonly IAPIModelConverterService modelConverterService;

        public PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger, IAPIModelConverterService modelConverterService, IInvoiceService invoiceService, IOrderService orderService)
        {
            this.logger = logger;
            this.modelConverterService = modelConverterService;
            this.paymentService = paymentService;
            this.invoiceService = invoiceService;
            this.orderService = orderService;
        }

       
        /*[HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilter filter, [FromServices] ODataQueryOptions<Payment> oDataOptions)
        {

            try
            {
                var settings = new ODataValidationSettings()
                {
                    AllowedFunctions = AllowedFunctions.AllFunctions
                };
                oDataOptions.Validate(settings);
                var data = await paymentService.GetAllAsync(filter, oDataOptions);
                var pagedReponse = PaginationHelper.CreatePagedReponse(Response, data.Data);
                return Ok(pagedReponse);
            }

            catch (Exception ex)
            {

                return BadRequest();
            }


        }

        [HttpGet("{id}")]
        public async Task<RestAPIGenericResponseDTO<Payment>> GetById(int id)
        {
            var resultResponse = new RestAPIGenericResponseDTO<Payment>();

            try
            {
                var data = await paymentService.GetPaymentByIdAsync(id);

                if (data != null)
                    resultResponse = new RestAPIGenericResponseDTO<Payment>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, data);
                else
                    resultResponse = new RestAPIGenericResponseDTO<Payment>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.PAYMENT_ID_NOT_AVAILABE);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<Payment>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<Payment>().WithException(ex);
            }

            return resultResponse;
        }

        [HttpGet("invoices/{invoiceNo}")]
        public async Task<RestAPIGenericResponseDTO<Payment>> GetById(string invoiceNo)
        {
            var resultResponse = new RestAPIGenericResponseDTO<Payment>();

            try
            {
                var data = await paymentService.GetPaymentByInvNoAsync(invoiceNo);

                if (data != null)
                    resultResponse = new RestAPIGenericResponseDTO<Payment>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, data);
                else
                    resultResponse = new RestAPIGenericResponseDTO<Payment>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.PAYMENT_ID_NOT_AVAILABE);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<Payment>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<Payment>().WithException(ex);
            }

            return resultResponse;
        }



        [HttpPost]
        public async Task<RestAPIGenericResponseDTO<PaymentResponseDto>> AddPayment (PaymentRequestDto requestDto)
        {
            var resultResponse = new RestAPIGenericResponseDTO<PaymentResponseDto>();

            try
            {
                logger.LogInformation("Payment Request DTO {}", requestDto.ToString());
                var validationError = requestDto.IsValid();
                if(!string.IsNullOrEmpty(validationError))
                {
                    return new RestAPIGenericResponseDTO<PaymentResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, validationError);
                }
                 var entity = await modelConverterService.ConvertToEntityModel<Payment>(requestDto);
                var paymentData = await paymentService.AddAsync(entity);
                if (paymentData.ReturnStatus == ReturnStatusEnum.Success)
                {
                    var orderResponse = new PaymentResponseDto().ToResponse(entity);
                    resultResponse = new RestAPIGenericResponseDTO<PaymentResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, orderResponse);
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


        [HttpPut("{id}/{status}")]
        public async Task<RestAPIGenericResponseDTO<string>> UpdateStatus(int id, PaymentStatusEnum status)
        {
            var resultResponse = new RestAPIGenericResponseDTO<string>();

            try
            {
                var data = await paymentService.GetPaymentByIdAsync(id);
                if (data == null)
                    return new RestAPIGenericResponseDTO<string>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.PAYMENT_ID_NOT_AVAILABE);

                var updateStatus = await paymentService.UpdateStatusAsync(id, (int)status);
                
                resultResponse = new RestAPIGenericResponseDTO<string>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, updateStatus);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<string>().WithSqlException(ex);
            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<string>().WithException(ex);
            }

            return resultResponse;

        }

        [HttpPost("{payReference}/confirm")]
        public async Task<RestAPIGenericResponseDTO<ConfirmPaymentResponseDto>> confirmPayment(string payReference, ConfirmPaymentRequestDto requestDto)
        {
            var resultResponse = new RestAPIGenericResponseDTO<ConfirmPaymentResponseDto>();
            
            try
            {

                var paymentData = await paymentService.GetPaymentByRefAsync(payReference);
                if (paymentData == null)
                    return new RestAPIGenericResponseDTO<ConfirmPaymentResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.PAYMENT_ID_NOT_AVAILABE);

                if (requestDto.TransactionStatus == TransactionStatusEnum.Success)
                {

                    string[] payMethods = { requestDto.PaymentMethod };
                    var paymentMethods = JSONHelper<string[]>.GetJSONStr(payMethods) ?? "";
                    // if the payment created by an order
                    if (string.IsNullOrEmpty(paymentData.InvoiceReference))
                    {
                        var addedInvoice = await invoiceService.AddPaidInvByOrderRef(paymentData.OrderReference, paymentMethods);

                        if (addedInvoice.Entity.OrderId > 0)
                        {
                            var isCompleted = await orderService.UpdateStatusAsync(int.Parse(addedInvoice.Entity.OrderId.ToString()), (int)OrderStatusEnum.Paid);
                        }
                    }
                    else
                    {
                        var invoiceInfo = await invoiceService.GetByInvoiceNo(paymentData.InvoiceReference);
                        InvoiceModel invoice = new()
                        {
                            AmountPaid = requestDto.Amount,
                            AmountRemaining = invoiceInfo.AmountRemaining - requestDto.Amount,
                            PaymentMethods = paymentMethods,
                            Status = (int)InvoiceStatusEnums.Paid                          

                        };

                        // Update the paidAmount, remainingAmount and status of the invoice
                        var updatePaidAmount = await invoiceService.UpdateAsync(int.Parse(invoiceInfo.Id.ToString()), invoice);
                    }

                    Payment payment = new()
                    {
                        Id = paymentData.Id,
                        TargetReference = requestDto.TargetReference,
                        TransactionReference = requestDto.TransactionReference,
                        PaymentMethod = requestDto.PaymentMethod,
                        PaymentStatus = PaymentStatusEnum.Success,
                        PayDate = DateTime.Now
                    };

                    // Update the status, paymentMethod and payment date of the payment
                    var updateStatus = await paymentService.UpdateAsync(int.Parse(paymentData.Id.ToString()), payment);
                                       
                    resultResponse = new RestAPIGenericResponseDTO<ConfirmPaymentResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message);
                }
                else
                {
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

        }*/



        // Scenario 1: Order-> Confirm Order + Create Invoice + Create payment -> Confirm payment
        /* [HttpPost("{invNo}/confirm")]
         public async Task<RestAPIGenericResponseDTO<ConfirmPaymentResponseDto>> confirmPayment(string invNo, ConfirmPaymentRequestDto requestDto)
         {
             var resultResponse = new RestAPIGenericResponseDTO<ConfirmPaymentResponseDto>();

             try
             {
                 var invoiceData = await invoiceService.GetByInvoiceNo(invNo);
                 if (invoiceData == null)
                     return new RestAPIGenericResponseDTO<ConfirmPaymentResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.INVOICE_NOT_AVAILABE);

                 var paymentData = await paymentService.GetPaymentByInvNoAsync(invNo);
                 if (paymentData == null)
                     return new RestAPIGenericResponseDTO<ConfirmPaymentResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.PAYMENT_ID_NOT_AVAILABE);

                 if(requestDto.TransactionStatus == TransactionStatusEnum.Success)
                 {
                     Payment payment = new()
                     {
                         Id = paymentData.Id,
                         TargetReference = requestDto.TargetReference,
                         TransactionReference = requestDto.TransactionReference,
                         PaymentMethod = requestDto.PaymentMethod,
                         PaymentStatus = PaymentStatusEnum.Success,
                         PayDate = DateTime.Now
                     };
                     // Update the status, paymentMethod and payment date of the payment
                     var updateStatus = await paymentService.UpdateAsync(int.Parse(paymentData.Id.ToString()), payment);

                     InvoiceModel invoice = new()
                     {
                         AmountPaid = requestDto.Amount,
                         AmountRemaining = invoiceData.AmountRemaining - requestDto.Amount,
                         PaymentMethods = requestDto.PaymentMethod,
                         Status = (int)InvoiceStatusEnums.Paid

                     };

                     // Update the paidAmount, remainingAmount and status of the invoice
                     var updatePaidAmount = await invoiceService.UpdateAsync(int.Parse(invoiceData.Id.ToString()), invoice);
                     if(invoiceData.OrderId > 0)
                     {
                         var isCompleted = await orderService.UpdateStatusAsync(invoiceData.OrderId, (int)OrderStatusEnum.Completed);
                     }                    
                     resultResponse = new RestAPIGenericResponseDTO<ConfirmPaymentResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message);
                 }
                 else
                 {
                     //var updateStatus = await paymentService.UpdateStatusAsync(int.Parse(paymentData.Id.ToString()), (int)PaymentStatusEnum.Failure);
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

         }*/

    }
}
