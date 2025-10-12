using Application.IServices;
using Application.Utils.ModelConverter;
using Domain.Entities;
using Domain.Models;
using Domain.Models.Base;
using Domain.Models.Services.RequestDto;
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
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> logger;
        private readonly IProductService service; 
        private readonly IAPIModelConverterService modelConverterService;

        public ProductsController(IProductService service, ILogger<ProductsController> logger, IAPIModelConverterService modelConverterService)
        {

            this.service = service;
            this.logger = logger;
            this.modelConverterService = modelConverterService;
        }

        /*[HttpGet("all")]
        public async Task<RestAPIGenericResponseDTO<IList<ProductModel>>> GetAll()
        {
            var resultResponse = new RestAPIGenericResponseDTO<IList<ProductModel>>();

            try
            {
                var filter = new GeneralFilterDto();
                var data = await service.GetAllAsync(filter);
                resultResponse = new RestAPIGenericResponseDTO<IList<ProductModel>>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, data);
            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<IList<ProductModel>>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<IList<ProductModel>>().WithException(ex);
            }

            return resultResponse;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilter filter, [FromServices] ODataQueryOptions<ProductModel> oDataOptions)
        {
            var resultResponse = new RestAPIGenericResponseDTO<IList<ProductModel>>();

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
        public async Task<RestAPIGenericResponseDTO<ProductModel>> GetById(int id)
        {
            var resultResponse = new RestAPIGenericResponseDTO<ProductModel>();

            try
            {
                var data = await service.GetByIdAsync(id);

                if (data != null)
                    resultResponse = new RestAPIGenericResponseDTO<ProductModel>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, data);
                else
                    resultResponse = new RestAPIGenericResponseDTO<ProductModel>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.PRODUCT_NOT_AVAILABE);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<ProductModel>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<ProductModel>().WithException(ex);
            }

            return resultResponse;
        }

        [HttpPost]
        public async Task<RestAPIGenericResponseDTO<ProductModel>> AddProduct(ProductRequestDto requestDto)
        {
            var resultResponse = new RestAPIGenericResponseDTO<ProductModel>();

            try
            {

                logger.LogInformation("Product Request DTO {}", requestDto.ToString());

                var validationErr = requestDto.IsValid();
                if (!string.IsNullOrEmpty(validationErr))
                {
                    // abort request processing & return error message
                    logger.LogInformation("Validation Error {}" + validationErr);
                    return new RestAPIGenericResponseDTO<ProductModel>().WithError(Constants.VALIDATION_ERROR_CODE, validationErr);
                }

                var entity = await modelConverterService.ConvertToEntityModel<ProductModel>(requestDto);
                var productData = await service.AddAsync(entity);
                resultResponse = new RestAPIGenericResponseDTO<ProductModel>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<ProductModel>().WithSqlException(ex);
            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<ProductModel>().WithException(ex);
            }

            return resultResponse;

        }

        [HttpPut("{id}/{status}")]
        public async Task<RestAPIGenericResponseDTO<ProductModel>> UpdateStatus(int id, ProductsStatusEnum status)
        {
            var resultResponse = new RestAPIGenericResponseDTO<ProductModel>();

            try
            {
                var data = await service.GetByIdAsync(id);
                if (data == null)
                    return new RestAPIGenericResponseDTO<ProductModel>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.PRODUCT_NOT_AVAILABE);

                var updateStatus = await service.UpdateStatusAsync(id, (int)status);
                resultResponse = new RestAPIGenericResponseDTO<ProductModel>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, updateStatus);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<ProductModel>().WithSqlException(ex);
            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<ProductModel>().WithException(ex);
            }

            return resultResponse;

        }

        [HttpDelete("{id}")]
        public async Task<RestAPIGenericResponseDTO<ProductModel>> DeletePtoduct(int id)
        {
            var resultResponse = new RestAPIGenericResponseDTO<ProductModel>();

            try
            {
                var data = await service.GetByIdAsync(id);
                if (data == null)
                    return new RestAPIGenericResponseDTO<ProductModel>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.PRODUCT_NOT_AVAILABE);

                var deleteProduct = await service.DeleteAsync(id);
                resultResponse = new RestAPIGenericResponseDTO<ProductModel>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, deleteProduct);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<ProductModel>().WithSqlException(ex);
            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<ProductModel>().WithException(ex);
            }

            return resultResponse;

        }
        [HttpPut("{id}")]
        public async Task<RestAPIGenericResponseDTO<string>> Update(int id, ProductRequestDto requestDto)
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
                    return new RestAPIGenericResponseDTO<string>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.PRODUCT_NOT_AVAILABE);

                var entity = await modelConverterService.ConvertToEntityModel<ProductModel>(requestDto);
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
