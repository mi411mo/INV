using Application.DTOs;
using Application.IServices;
using Application.IWebServices;
using Application.Utils.ModelConverter;
using Domain.Entities;
using Domain.Models;
using Domain.Models.Orders.RequestDto;
using Domain.Models.Orders.ResponseDto;
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
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> logger;
        private readonly IOrderService orderService ;
        private readonly IInvoiceService invoiceService;
        private readonly IMerchantService merchantService;
        private readonly IPaymentService paymentService;
        private readonly IPaymentGatewayWebService redirectPaymentService;
        private readonly IAPIModelConverterService modelConverterService;

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger, IAPIModelConverterService modelConverterService, IInvoiceService invoiceService, 
                                IPaymentGatewayWebService redirectPaymentService, IMerchantService merchantService, IPaymentService paymentService)
        {

            this.orderService = orderService;
            this.invoiceService = invoiceService;
            this.paymentService = paymentService;
            this.logger = logger;
            this.modelConverterService = modelConverterService;
            this.redirectPaymentService = redirectPaymentService;
            this.merchantService = merchantService;
        }

        /* [HttpGet]
         public async Task<RestAPIGenericResponseDTO<IList<OrderModel>>> GetAll()
         {
             var resultResponse = new RestAPIGenericResponseDTO<IList<OrderModel>>();

             try
             {
                 var data = await orderService.GetAllOrdersAsync();
                 resultResponse = new RestAPIGenericResponseDTO<IList<OrderModel>>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, data);
             }
             catch (SqlException ex)
             {
                 resultResponse = new RestAPIGenericResponseDTO<IList<OrderModel>>().WithSqlException(ex);

             }
             catch (Exception ex)
             {
                 resultResponse = new RestAPIGenericResponseDTO<IList<OrderModel>>().WithException(ex);
             }

             return resultResponse;
         }*/
       /* [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilter filter, [FromServices] ODataQueryOptions<OrderModel> oDataOptions)
        {

            try
            {
                var settings = new ODataValidationSettings()
                {
                    AllowedFunctions = AllowedFunctions.AllFunctions
                };
                oDataOptions.Validate(settings);
                var data = await orderService.GetAllAsync(filter, oDataOptions);
                var pagedReponse = PaginationHelper.CreatePagedReponse(Response, data.Data);
                return Ok(pagedReponse);
            }

            catch (Exception ex)
            {

                return BadRequest();
            }


        }

        [HttpGet("{id}")]
        public async Task<RestAPIGenericResponseDTO<OrderModel>> GetById(int id)
        {
            var resultResponse = new RestAPIGenericResponseDTO<OrderModel>();

            try
            {
                var data = await orderService.GetOrderByIdAsync(id);

                if (data != null)
                    resultResponse = new RestAPIGenericResponseDTO<OrderModel>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, data);
                else
                    resultResponse = new RestAPIGenericResponseDTO<OrderModel>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.ORDER_ID_NOT_AVAILABE);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<OrderModel>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<OrderModel>().WithException(ex);
            }

            return resultResponse;
        }


        [HttpGet("{orderReference}/details")]
        public async Task<RestAPIGenericResponseDTO<OrderModel>> GetByRef(string orderReference)
        {
            var resultResponse = new RestAPIGenericResponseDTO<OrderModel>();

            try
            {
                var data = await orderService.GetOrderByRefAsync(orderReference);

                if (data != null)
                    resultResponse = new RestAPIGenericResponseDTO<OrderModel>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, data);
                else
                    resultResponse = new RestAPIGenericResponseDTO<OrderModel>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.ORDER_ID_NOT_AVAILABE);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<OrderModel>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<OrderModel>().WithException(ex);
            }

            return resultResponse;
        }


        [HttpPost]
        public async Task<RestAPIGenericResponseDTO<OrderResponseDto>> AddOrder (OrderRequestDto requestDto)
        {
            var resultResponse = new RestAPIGenericResponseDTO<OrderResponseDto>();

            try
            {
                logger.LogInformation("Orders Request DTO {}", requestDto.ToString());
                var validationError = requestDto.IsValid();
                if(!string.IsNullOrEmpty(validationError))
                {
                    return new RestAPIGenericResponseDTO<OrderResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, validationError);
                }
                 var entity = await modelConverterService.ConvertToEntityModel<OrderModel>(requestDto);
                var orderData = await orderService.AddAsync(entity);
                if (orderData.ReturnStatus == ReturnStatusEnum.Success)
                {
                    var orderResponse = new OrderResponseDto().ToResponse(entity);
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
        [HttpPut("{id}/{status}")]
        public async Task<RestAPIGenericResponseDTO<string>> UpdateStatus(int id, OrderStatusEnum status)
        {
            var resultResponse = new RestAPIGenericResponseDTO<string>();

            try
            {
                var data = await orderService.GetOrderByIdAsync(id);
                if (data == null)
                    return new RestAPIGenericResponseDTO<string>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.ORDER_ID_NOT_AVAILABE);

                var updateStatus = await orderService.UpdateStatusAsync(id, (int)status);
                
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

        //Scenario 2: Create Order-> Confirm Order + Create payment -> Confirm payment
        [HttpPut("confirm")]
        public async Task<RestAPIGenericResponseDTO<OrderConfirmResponseDto>> ConfirmOrder([FromQuery] string orderRef)
        {
            var resultResponse = new RestAPIGenericResponseDTO<OrderConfirmResponseDto>();

            try
            {
                var orderData = await orderService.GetOrderByRefAsync(orderRef);
                if (orderData == null)
                    return new RestAPIGenericResponseDTO<OrderConfirmResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.ORDER_REF_NOT_AVAILABE);

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

        }*/


        //Scenario 1: Order-> Confirm Order + Create Invoice + Create payment -> Confirm payment
        /*[HttpPut("confirm")]
        public async Task<RestAPIGenericResponseDTO<OrderConfirmResponseDto>> ConfirmOrder([FromQuery]string orderRef)
        {
            var resultResponse = new RestAPIGenericResponseDTO<OrderConfirmResponseDto>();
            
            try
            {
                var data = await orderService.GetOrderByRefAsync(orderRef);
                if (data == null)
                    return new RestAPIGenericResponseDTO<OrderConfirmResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.ORDER_REF_NOT_AVAILABE);

                bool isConfirmed = await orderService.ConfirmOrderAsync(orderRef);
                if(isConfirmed)
                {
                    var invoice =  await invoiceService.AddByOrderRef(orderRef);
                    var merchantInfo = await merchantService.GetByIdAsync(invoice.Entity.MerchantId);
                    var payment = await paymentService.AddByInvNoAsync(invoice.Entity.InvoiceNumber);
                    var redirectReq = new RedirectLinkPaymentsRequest().ToModel(invoice.Entity, merchantInfo, payment.Entity);
                    var redirectRes = await redirectPaymentService.RedirectLinkPayments(redirectReq);
                    var responseDto = new OrderConfirmResponseDto().ToResponse(invoice.Entity, redirectRes.Entity.TransactionUrl);
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

        }*/
    }
}
