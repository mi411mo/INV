using Application.Interfaces.IBusinessIndependentService.IServices;
using Application.Interfaces.IBusinessLogic;
using Application.Utils.ModelConverter;
using Domain.Entities;
using Domain.Models;
using Domain.Models.Base;
using Domain.Models.Categories;
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
    public class CategoryBusinessServiceImpl : ICategoryBusinessService
    {
        private readonly ICategoryService categoryService;
        private readonly ILogger<CategoryBusinessServiceImpl> logger;
        private readonly IAPIModelConverterService modelConverterService;
        private readonly IAuditLogService<Category> auditLogService;

        public CategoryBusinessServiceImpl(ICategoryService categoryService, ILogger<CategoryBusinessServiceImpl> logger, IAPIModelConverterService modelConverterService, IAuditLogService<Category> auditLogService)
        {
            this.categoryService = categoryService;
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
                var data = await categoryService.GetByIdAsync(id);
                if (data == null)
                    return new RestAPIGenericResponseDTO<object>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.CATEGORY_NOT_AVAILABE);

                var deleteCustomer = await categoryService.DeleteAsync(id);
                await auditLogService.DeleteActionAsync(profileInfo, data);
                resultResponse = new RestAPIGenericResponseDTO<object>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, deleteCustomer);

            }
            catch (SqlException ex)
            {
                if(ex.Number == Constants.FOREIGN_KEY_VIOLATION_ERROR_CODE)
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

        public async Task<RestAPIGenericResponseDTO<CategoryResponseDto>> ExecuteAsync(CategoryRequestDto request, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<CategoryResponseDto>();

            try
            {

                logger.LogInformation("Category Request DTO {}", request.ToString());

                var validationErr = request.IsValid();
                if (!string.IsNullOrEmpty(validationErr))
                {
                    // abort request processing & return error message
                    logger.LogInformation("Validation Error {}" + validationErr);
                    return new RestAPIGenericResponseDTO<CategoryResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, validationErr);
                }

                var entity = await modelConverterService.ConvertToEntityModel <Category>(request);
                var customerData = await categoryService.AddAsync(entity);
                if (customerData.ReturnStatus == ReturnStatusEnum.Success)
                {
                    await auditLogService.CreateActionAsync(profileInfo, entity);
                    resultResponse = new RestAPIGenericResponseDTO<CategoryResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message);
                }
                else
                    resultResponse = new RestAPIGenericResponseDTO<CategoryResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, customerData.Message);


            }
            catch (SqlException ex)
            {
                if (ex.Number == Constants.FOREIGN_KEY_VIOLATION_ERROR_CODE)
                    resultResponse = new RestAPIGenericResponseDTO<CategoryResponseDto>().WithForeignKeyViolationException(ex, Constants.FOREIGN_KEY_INSERT_VIOLATION_MESSAGE + "تأكد من رقم التاجر");
                else
                    resultResponse = new RestAPIGenericResponseDTO<CategoryResponseDto>().WithSqlException(ex);
            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<CategoryResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public async Task<RestAPIGenericResponseDTO<CategoryResponseDto>> GetAll(GeneralFilterDto generalFilter, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<CategoryResponseDto>();

            try
            {
                var data = await categoryService.GetAllAsync(generalFilter);
                var ignore = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                var JSONString = JsonConvert.SerializeObject(data, ignore);
                var responseData = JsonConvert.DeserializeObject<IList<CategoryResponseDto>>(JSONString);
                resultResponse = new RestAPIGenericResponseDTO<CategoryResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, responseData);
            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<CategoryResponseDto>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<CategoryResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public async Task<RestAPIGenericResponseDTO<CategoryResponseDto>> GetById(int id, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<CategoryResponseDto>();

            try
            {
                var data = await categoryService.GetByIdAsync(id);

                if (data != null)
                {
                    var customerResponse = await modelConverterService.ConvertToResponseDto<CategoryResponseDto>(data);
                    resultResponse = new RestAPIGenericResponseDTO<CategoryResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, customerResponse);
                }
                else
                    resultResponse = new RestAPIGenericResponseDTO<CategoryResponseDto>().WithSuccess(Constants.VALIDATION_ERROR_CODE, Constants.CATEGORY_NOT_AVAILABE);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<CategoryResponseDto>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<CategoryResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public async Task<int> GetCountAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                return await categoryService.GetCountAsync(generalFilter);
               
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<RestAPIGenericResponseDTO<object>> UpdateAsync(CategoryRequestDto request, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
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
                var data = await categoryService.GetByIdAsync(request.Id);
                if (data == null)
                    return new RestAPIGenericResponseDTO<object>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.CATEGORY_NOT_AVAILABE);

                var entity = await modelConverterService.ConvertToEntityModel<Category>(request);

                await auditLogService.UpdateActionAsync(profileInfo, data, entity);
                var updateProduct = await categoryService.UpdateAsync(request.Id, entity);

                resultResponse = new RestAPIGenericResponseDTO<object>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message);

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

        public async Task<RestAPIGenericResponseDTO<object>> UpdateStatusAsync(int id, int status, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<object>();

            try
            {
                var data = await categoryService.GetByIdAsync(id);
                if (data == null)
                    return new RestAPIGenericResponseDTO<object>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.CATEGORY_NOT_AVAILABE);

                var updateStatus = await categoryService.UpdateStatusAsync(id, status);

                await auditLogService.UpdateStatusActionAsync(profileInfo, data.IsActive.ToString(), status.ToString());
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
