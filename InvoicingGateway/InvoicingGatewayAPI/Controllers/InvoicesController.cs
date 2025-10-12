using Application.DTOs;
using Application.IServices;
using Application.IWebServices;
using Application.Utils.ModelConverter;
using Domain.Entities;
using Domain.Models;
using Domain.Models.Invoices.RequestDto;
using Domain.Models.Invoices.ResponseDto;
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
    public class InvoicesController : ControllerBase
    {
        private readonly ILogger<InvoicesController> logger;
        private readonly IInvoiceService invoiceService;
        private readonly IOrderService orderService;
        private readonly IPaymentService paymentService;
        private readonly IMerchantService merchantService;
        private readonly IPaymentGatewayWebService redirectPaymentService;
        private readonly IAPIModelConverterService modelConverterService;

        public InvoicesController(IInvoiceService invoiceService, ILogger<InvoicesController> logger, IAPIModelConverterService modelConverterService, IOrderService orderService, IPaymentService paymentService, IMerchantService merchantService, IPaymentGatewayWebService redirectPaymentService)
        {

            this.invoiceService = invoiceService;
            this.orderService = orderService;
            this.logger = logger;
            this.modelConverterService = modelConverterService;
            this.paymentService = paymentService;
            this.merchantService = merchantService;
            this.redirectPaymentService = redirectPaymentService;
        }

        /*[HttpGet]
        public async Task<RestAPIGenericResponseDTO<IList<InvoiceModel>>> GetAll()
        {
            var resultResponse = new RestAPIGenericResponseDTO<IList<InvoiceModel>>();

            try
            {
                var data = await invoiceService.GetAllAsync();
                resultResponse = new RestAPIGenericResponseDTO<IList<InvoiceModel>>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, data);
            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<IList<InvoiceModel>>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<IList<InvoiceModel>>().WithException(ex);
            }
            return resultResponse;
        }*/

       /* [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilter filter, [FromServices] ODataQueryOptions<InvoiceModel> oDataOptions)
        {

            try
            {
                var settings = new ODataValidationSettings()
                {
                    AllowedFunctions = AllowedFunctions.AllFunctions
                };
                oDataOptions.Validate(settings);
                var invData = await invoiceService.GetAllAsync(filter, oDataOptions);
                var pagedReponse = PaginationHelper.CreatePagedReponse(Response, invData.Data);
                return Ok(pagedReponse);
            }

            catch (Exception ex)
            {

                return BadRequest();
            }


        }

        [HttpGet("{id}")]
        public async Task<RestAPIGenericResponseDTO<InvoiceResponseDto>> GetById(int id)
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

        [HttpGet]
        [Route("{paymentToken}/details")]
        public async Task<RestAPIGenericResponseDTO<InvoiceResponseDto>> GetByPayToken(int paymentToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<InvoiceResponseDto>();

            try
            {
                var invData = await invoiceService.GetByTokenAsync(paymentToken);

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

        [HttpGet]
        [Route("{invoiceNumber}/info")]
        public async Task<RestAPIGenericResponseDTO<InvoiceResponseDto>> GetByInoviceNo(string invoiceNumber)
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


        [HttpPost]
        public async Task<RestAPIGenericResponseDTO<InvoiceResponseDto>> AddInvoice(InvoiceRequestDto requestDto)
        {
            var resultResponse = new RestAPIGenericResponseDTO<InvoiceResponseDto>();

            try
            {
                logger.LogInformation("Invoice Request DTO {}", requestDto.ToString());
                var validationErr = requestDto.IsValid();
                if (!string.IsNullOrEmpty(validationErr))
                {
                    // abort request processing & return error message
                    logger.LogInformation("Validation Error {}" + validationErr);
                    return new RestAPIGenericResponseDTO<InvoiceResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, validationErr);
                    
                }
                var entity = await modelConverterService.ConvertToEntityModel<InvoiceModel>(requestDto);
                //TODO: I should check the if there is unpaid paymentToken 
                var addInvoice = await invoiceService.AddAsync(entity);
                // We should use GetByInvoiceNumber
                var invoiceEntity = await invoiceService.GetByInvoiceNo(entity.InvoiceNumber.ToString());

                var invoiceData = await modelConverterService.ConvertToResponseDto<InvoiceResponseDto>(invoiceEntity);

                resultResponse = new RestAPIGenericResponseDTO<InvoiceResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, invoiceData);
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

        [HttpPut("{id}/{status}")]
        public async Task<RestAPIGenericResponseDTO<string>> UpdateStatus(int id, InvoiceStatusEnums status)
        {
            var resultResponse = new RestAPIGenericResponseDTO<string>();

            try
            {
                var data = await invoiceService.GetByIdAsync(id);
                if (data == null)
                    return new RestAPIGenericResponseDTO<string>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.INVOICE_NOT_AVAILABE);

                var updateStatus = await invoiceService.UpdateStatusAsync(id, (int)status);

                resultResponse = new RestAPIGenericResponseDTO<string>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.INVOICE_UPDATED_SUCCESSFULLY);

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

        [HttpPut("confirm")]
        public async Task<RestAPIGenericResponseDTO<InvoiceConfirmResponseDto>> ConfirmInvoice([FromQuery]string invoiceNo)
        {
            var resultResponse = new RestAPIGenericResponseDTO<InvoiceConfirmResponseDto>();

            try
            {
                var invData = await invoiceService.GetByInvoiceNo(invoiceNo);
                if (invData == null)
                    return new RestAPIGenericResponseDTO<InvoiceConfirmResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.INVOICE_NOT_AVAILABE);

                var isUpdated = await invoiceService.UpdateStatusAsync(int.Parse(invData.Id.ToString()), (int)InvoiceStatusEnums.Unpaid);

                if(isUpdated)
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

        }*/

    }
}
