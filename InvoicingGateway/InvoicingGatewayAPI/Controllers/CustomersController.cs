using Application.IServices;
using Application.Utils.ModelConverter;
using Domain.Entities;
using Domain.Models;
using Domain.Models.Customers;
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
    public class CustomersController : ControllerBase
    {
        private readonly ILogger<CustomersController> logger;
        private readonly ICustomerService service;
        private readonly IAPIModelConverterService modelConverterService;

        public CustomersController(ICustomerService service, ILogger<CustomersController> logger, IAPIModelConverterService modelConverterService)
        {

            this.service = service;
            this.logger = logger;
            this.modelConverterService = modelConverterService;
        }

        /*[HttpGet]
        //[EnableQuery]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.Filter | AllowedQueryOptions.OrderBy | AllowedQueryOptions.Select | AllowedQueryOptions.Expand)]
        public async Task<RestAPIGenericResponseDTO<IList<Customer>>> GetAll()
        { 
            var resultResponse = new RestAPIGenericResponseDTO<IList<Customer>>();

            try
            {
                var data = await service.GetAllAsync();
                resultResponse = new RestAPIGenericResponseDTO<IList<Customer>>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, data);
            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<IList<Customer>>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<IList<Customer>>().WithException(ex);
            }
            return resultResponse;
        }*/

       /* [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilter filter, [FromServices] ODataQueryOptions<Customer> oDataOptions)
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
        public async Task<RestAPIGenericResponseDTO<Customer>> GetById(int id)
        {
            var resultResponse = new RestAPIGenericResponseDTO<Customer>();

            try
            {
                var data = await service.GetByIdAsync(id);

                if (data != null)
                    resultResponse = new RestAPIGenericResponseDTO<Customer>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, data);
                else
                    resultResponse = new RestAPIGenericResponseDTO<Customer>().WithSuccess(Constants.VALIDATION_ERROR_CODE, Constants.Customer_NOT_AVAILABE);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<Customer>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<Customer>().WithException(ex);
            }

            return resultResponse;
        }

        [HttpPost]
        public async Task<RestAPIGenericResponseDTO<Customer>> AddCustomer (CustomerRequestDto requestDto)
        {
            var resultResponse = new RestAPIGenericResponseDTO<Customer>();

            try
            {

                logger.LogInformation("Customer Request DTO {}", requestDto.ToString());

                var validationErr = requestDto.IsValid();
                if (!string.IsNullOrEmpty(validationErr))
                {
                    // abort request processing & return error message
                    logger.LogInformation("Validation Error {}" + validationErr);
                    return new RestAPIGenericResponseDTO<Customer>().WithError(Constants.VALIDATION_ERROR_CODE, validationErr);
                }

                var entity = await modelConverterService.ConvertToEntityModel<Customer>(requestDto);
                var customerData = await service.AddAsync(entity);
                if(customerData.ReturnStatus == ReturnStatusEnum.Success)
                {
                    resultResponse = new RestAPIGenericResponseDTO<Customer>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message);
                }
                else
                    resultResponse = new RestAPIGenericResponseDTO<Customer>().WithError(Constants.VALIDATION_ERROR_CODE, customerData.Message);


            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<Customer>().WithSqlException(ex);
            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<Customer>().WithException(ex);
            }

            return resultResponse;

        }

        [HttpPut("{id}/{status}")]
        public async Task<RestAPIGenericResponseDTO<Customer>> UpdateStatus(int id, int status)
        {
            var resultResponse = new RestAPIGenericResponseDTO<Customer>();

            try
            {
                var data = await service.GetByIdAsync(id);
                if (data == null)
                    return new RestAPIGenericResponseDTO<Customer>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.Customer_NOT_AVAILABE);

                var updateStatus = await service.UpdateStatusAsync(id, status);
                resultResponse = new RestAPIGenericResponseDTO<Customer>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, updateStatus);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<Customer>().WithSqlException(ex);
            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<Customer>().WithException(ex);
            }

            return resultResponse;

        }

        [HttpDelete("{id}")]
        public async Task<RestAPIGenericResponseDTO<Customer>> DeleteCustomer(int id)
        {
            var resultResponse = new RestAPIGenericResponseDTO<Customer>();

            try
            {
                var data = await service.GetByIdAsync(id);
                if (data == null)
                    return new RestAPIGenericResponseDTO<Customer>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.Customer_NOT_AVAILABE);

                var deleteCustomer = await service.DeleteAsync(id);
                resultResponse = new RestAPIGenericResponseDTO<Customer>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, deleteCustomer);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<Customer>().WithSqlException(ex);
            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<Customer>().WithException(ex);
            }

            return resultResponse;

        }
        [HttpPut("{id}")]
        public async Task<RestAPIGenericResponseDTO<string>> Update(int id, CustomerRequestDto requestDto)
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
                    return new RestAPIGenericResponseDTO<string>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.Customer_NOT_AVAILABE);

                var entity = await modelConverterService.ConvertToEntityModel<Customer>(requestDto);
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
