using Application.IServices;
using Application.Utils.ModelConverter;
using Domain.Entities;
using Domain.Models;
using Domain.Models.Customers;
using Domain.Models.Merchants;
using Domain.Utils;
using Domain.Utils.Enums;
using InvoicingGatewayAPI.Utils;
using Microsoft.AspNet.OData;
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
    public class MerchantsController : ControllerBase
    {
        private readonly ILogger<MerchantsController> logger;
        private readonly IMerchantService service;
        private readonly IAPIModelConverterService modelConverterService;

        public MerchantsController(IMerchantService service, ILogger<MerchantsController> logger, IAPIModelConverterService modelConverterService)
        {

            this.service = service;
            this.logger = logger;
            this.modelConverterService = modelConverterService;
        }

       /* [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilter filter, [FromServices] ODataQueryOptions<Merchant> oDataOptions)
        {

            try
            {
                var settings = new ODataValidationSettings()
                {
                    AllowedFunctions = AllowedFunctions.AllFunctions
                };
                oDataOptions.Validate(settings);
                var data = await service.GetAllAsync(filter, oDataOptions);
                var pagedReponse = PaginationHelper.CreatePagedReponse(Response, data.Data);
                return Ok(pagedReponse);
            }

            catch (Exception ex)
            {

                return BadRequest();
            }


        }

        [HttpGet("{id}")]
        public async Task<RestAPIGenericResponseDTO<Merchant>> GetById(int id)
        {
            var resultResponse = new RestAPIGenericResponseDTO<Merchant>();

            try
            {
                var data = await service.GetByIdAsync(id);

                if (data != null)
                    resultResponse = new RestAPIGenericResponseDTO<Merchant>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, data);
                else
                    resultResponse = new RestAPIGenericResponseDTO<Merchant>().WithSuccess(Constants.VALIDATION_ERROR_CODE, Constants.MERCHANT_NOT_AVAILABE);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<Merchant>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<Merchant>().WithException(ex);
            }

            return resultResponse;
        }

        [HttpPost]
        public async Task<RestAPIGenericResponseDTO<Merchant>> AddMerchant (MerchantRequestDto requestDto)
        {
            var resultResponse = new RestAPIGenericResponseDTO<Merchant>();

            try
            {

                logger.LogInformation("Merchant Request DTO {}", requestDto.ToString());

                var validationErr = requestDto.IsValid();
                if (!string.IsNullOrEmpty(validationErr))
                {
                    // abort request processing & return error message
                    logger.LogInformation("Validation Error {}" + validationErr);
                    return new RestAPIGenericResponseDTO<Merchant>().WithError(Constants.VALIDATION_ERROR_CODE, validationErr);
                }

                var entity = await modelConverterService.ConvertToEntityModel<Merchant>(requestDto);
                var merchantData = await service.AddAsync(entity);
                if(merchantData.ReturnStatus == ReturnStatusEnum.Success)
                {
                    resultResponse = new RestAPIGenericResponseDTO<Merchant>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message);
                }
                else
                    resultResponse = new RestAPIGenericResponseDTO<Merchant>().WithError(Constants.VALIDATION_ERROR_CODE, merchantData.Message);


            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<Merchant>().WithSqlException(ex);
            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<Merchant>().WithException(ex);
            }

            return resultResponse;

        }

        [HttpPut("{id}/{status}")]
        public async Task<RestAPIGenericResponseDTO<Merchant>> UpdateStatus(int id, int status)
        {
            var resultResponse = new RestAPIGenericResponseDTO<Merchant>();

            try
            {
                var data = await service.GetByIdAsync(id);
                if (data == null)
                    return new RestAPIGenericResponseDTO<Merchant>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.MERCHANT_NOT_AVAILABE);

                var updateStatus = await service.UpdateStatusAsync(id, status);
                resultResponse = new RestAPIGenericResponseDTO<Merchant>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, updateStatus);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<Merchant>().WithSqlException(ex);
            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<Merchant>().WithException(ex);
            }

            return resultResponse;

        }

        [HttpDelete("{id}")]
        public async Task<RestAPIGenericResponseDTO<Merchant>> DeleteMerchant(int id)
        {
            var resultResponse = new RestAPIGenericResponseDTO<Merchant>();

            try
            {
                var data = await service.GetByIdAsync(id);
                if (data == null)
                    return new RestAPIGenericResponseDTO<Merchant>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.MERCHANT_NOT_AVAILABE);

                var deleteCustomer = await service.DeleteAsync(id);
                resultResponse = new RestAPIGenericResponseDTO<Merchant>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, deleteCustomer);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<Merchant>().WithSqlException(ex);
            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<Merchant>().WithException(ex);
            }

            return resultResponse;

        }
        [HttpPut("{id}")]
        public async Task<RestAPIGenericResponseDTO<string>> Update(int id, MerchantRequestDto requestDto)
        {
            var resultResponse = new RestAPIGenericResponseDTO<string>();

            try
            {
                var validationErr = requestDto.IsValid();
                if (!string.IsNullOrEmpty(validationErr))
                {
                    // abort request processing & return error message
                    logger.LogInformation("Validation Error {}" + validationErr);
                    return new RestAPIGenericResponseDTO<string>().WithError(Constants.VALIDATION_ERROR_CODE, validationErr);
                }
                var data = await service.GetByIdAsync(id);
                if (data == null)
                    return new RestAPIGenericResponseDTO<string>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.MERCHANT_NOT_AVAILABE);

                var entity = await modelConverterService.ConvertToEntityModel<Merchant>(requestDto);
                var updateProduct = await service.UpdateAsync(id, entity);

                resultResponse = new RestAPIGenericResponseDTO<string>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message);

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

        }*/
    }
}
