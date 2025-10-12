using Application.Interfaces.IBusinessIndependentService.IServices;
using Application.Interfaces.IBusinessLogic;
using Application.IServices;
using Application.Utils.ModelConverter;
using Domain.Entities;
using Domain.Models;
using Domain.Models.Base;
using Domain.Models.Services;
using Domain.Models.Services.RequestDto;
using Domain.Utils;
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
    public class ProductBusinessServiceImpl : IProductBusinessService
    {
        private readonly IProductService productService;
        private readonly ILogger<ProductBusinessServiceImpl> logger;
        private readonly IAPIModelConverterService modelConverterService;
        private readonly IAuditLogService<ProductModel> auditLogService;

        public ProductBusinessServiceImpl(IProductService productService, ILogger<ProductBusinessServiceImpl> logger, IAPIModelConverterService modelConverterService, IAuditLogService<ProductModel> auditLogService)
        {
            this.productService = productService;
            this.logger = logger;
            this.modelConverterService = modelConverterService;
            this.auditLogService = auditLogService;
        }
        public Task<RestAPIGenericResponseDTO<object>> ChangeStatusAsync(int id, int status, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<RestAPIGenericResponseDTO<object>> DeleteAsync(int id, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<object>();

            try
            {
                var data = await productService.GetByIdAsync(id);
                if (data == null)
                    return new RestAPIGenericResponseDTO<object>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.PRODUCT_NOT_AVAILABE);

                var deleteProduct = await productService.DeleteAsync(id);

                await auditLogService.DeleteActionAsync(profileInfo, data);
                resultResponse = new RestAPIGenericResponseDTO<object>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, deleteProduct);

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

        public async Task<RestAPIGenericResponseDTO<ProductResponseDto>> ExecuteAsync(ProductRequestDto request, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<ProductResponseDto>();

            try
            {

                logger.LogInformation("Product Request DTO {}", request.ToString());

                var validationErr = request.IsValid();
                if (!string.IsNullOrEmpty(validationErr))
                {
                    // abort request processing & return error message
                    logger.LogInformation("Validation Error {}" + validationErr);
                    return new RestAPIGenericResponseDTO<ProductResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, validationErr);
                }

                var entity = await modelConverterService.ConvertToEntityModel<ProductModel>(request);
                var productData = await productService.AddAsync(entity);
                await auditLogService.CreateActionAsync(profileInfo, entity);
                resultResponse = new RestAPIGenericResponseDTO<ProductResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<ProductResponseDto>().WithSqlException(ex);
            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<ProductResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public async Task<RestAPIGenericResponseDTO<ProductResponseDto>> GetAll(GeneralFilterDto generalFilter, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<ProductResponseDto>();

            try
            {
                var data = await productService.GetAllAsync(generalFilter);
                var ignore = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                var JSONString = JsonConvert.SerializeObject(data, ignore);
                var responseData = JsonConvert.DeserializeObject<IList<ProductResponseDto>>(JSONString);
                resultResponse = new RestAPIGenericResponseDTO<ProductResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, responseData);
            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<ProductResponseDto>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<ProductResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public async Task<RestAPIGenericResponseDTO<ProductResponseDto>> GetById(int id, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<ProductResponseDto>();

            try
            {
                var data = await productService.GetByIdAsync(id);

                if (data != null)
                {
                    var productResponse = await modelConverterService.ConvertToResponseDto<ProductResponseDto>(data);
                    resultResponse = new RestAPIGenericResponseDTO<ProductResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, productResponse);
                }                  
                else
                    resultResponse = new RestAPIGenericResponseDTO<ProductResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.PRODUCT_NOT_AVAILABE);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<ProductResponseDto>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<ProductResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public async Task<int> GetCountAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                return await productService.GetCountAsync(generalFilter);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<RestAPIGenericResponseDTO<object>> UpdateAsync(ProductRequestDto request, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<object>();

            try
            {
                var validationErr = request.IsValid();
                if (!string.IsNullOrEmpty(validationErr))
                {
                    // abort request processing & return error message
                    logger.LogInformation("Validation Error {}" + validationErr);
                    return new RestAPIGenericResponseDTO<object>().WithError(Constants.VALIDATION_ERROR_CODE, validationErr);
                }
                var data = await productService.GetByIdAsync(request.Id);
                if (data == null)
                    return new RestAPIGenericResponseDTO<object>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.PRODUCT_NOT_AVAILABE);

                var entity = await modelConverterService.ConvertToEntityModel<ProductModel>(request);
                var updateProduct = await productService.UpdateAsync(request.Id, entity);

                await auditLogService.UpdateActionAsync(profileInfo, data, entity);
                resultResponse = new RestAPIGenericResponseDTO<object>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message);

            }
            catch (SqlException ex)
            {
                if (ex.Number == Constants.FOREIGN_KEY_VIOLATION_ERROR_CODE)
                    resultResponse = new RestAPIGenericResponseDTO<object>().WithForeignKeyViolationException(ex, Constants.FOREIGN_KEY_DELETE_VIOLATION_MESSAGE);
                else
                    resultResponse = new RestAPIGenericResponseDTO<object>().WithSqlException(ex);
            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<object>().WithException(ex);
            }

            return resultResponse;
        }

        public async Task<RestAPIGenericResponseDTO<object>> UpdateStatusAsync(int id, int status, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<object>();

            try
            {
                var data = await productService.GetByIdAsync(id);
                if (data == null)
                    return new RestAPIGenericResponseDTO<object>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.PRODUCT_NOT_AVAILABE);

                var updateStatus = await productService.UpdateStatusAsync(id, (int)status);

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
    }
}
