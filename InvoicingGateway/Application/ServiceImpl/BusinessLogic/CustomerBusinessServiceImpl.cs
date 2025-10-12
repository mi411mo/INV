using Application.Interfaces.IBusinessIndependentService.IServices;
using Application.Interfaces.IBusinessLogic;
using Application.IServices;
using Application.Utils.ModelConverter;
using Domain.Entities;
using Domain.Models;
using Domain.Models.Base;
using Domain.Models.Customers;
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
    public class CustomerBusinessServiceImpl : ICustomerBusinessService
    {
        private readonly ICustomerService customerService;
        private readonly ILogger<CustomerBusinessServiceImpl> logger;
        private readonly IAPIModelConverterService modelConverterService;
        private readonly IAuditLogService<Customer> auditLogService;

        public CustomerBusinessServiceImpl(ICustomerService customerService, ILogger<CustomerBusinessServiceImpl> logger, IAPIModelConverterService modelConverterService, IAuditLogService<Customer> auditLogService)
        {
            this.customerService = customerService;
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
                var data = await customerService.GetByIdAsync(id);
                if (data == null)
                    return new RestAPIGenericResponseDTO<object>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.Customer_NOT_AVAILABE);

                var deleteCustomer = await customerService.DeleteAsync(id);
                await auditLogService.DeleteActionAsync(profileInfo, data);
                resultResponse = new RestAPIGenericResponseDTO<object>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, deleteCustomer);

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

        public async Task<RestAPIGenericResponseDTO<CustomerResponseDto>> ExecuteAsync(CustomerRequestDto request, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<CustomerResponseDto>();

            try
            {

                logger.LogInformation("Customer Request DTO {}", request.ToString());

                var validationErr = request.IsValid();
                if (!string.IsNullOrEmpty(validationErr))
                {
                    // abort request processing & return error message
                    logger.LogInformation("Validation Error {}" + validationErr);
                    return new RestAPIGenericResponseDTO<CustomerResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, validationErr);
                }

                var entity = await modelConverterService.ConvertToEntityModel<Customer>(request);
                var customerData = await customerService.AddAsync(entity);
                if (customerData.ReturnStatus == ReturnStatusEnum.Success)
                {
                    await auditLogService.CreateActionAsync(profileInfo, entity);
                    resultResponse = new RestAPIGenericResponseDTO<CustomerResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message);
                }
                else
                    resultResponse = new RestAPIGenericResponseDTO<CustomerResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, customerData.Message);


            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<CustomerResponseDto>().WithSqlException(ex);
            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<CustomerResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public async Task<RestAPIGenericResponseDTO<CustomerResponseDto>> GetAll(GeneralFilterDto generalFilter, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<CustomerResponseDto>();

            try
            {
                var data = await customerService.GetAllAsync(generalFilter);
                var ignore = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                var JSONString = JsonConvert.SerializeObject(data, ignore);
                var responseData = JsonConvert.DeserializeObject<IList<CustomerResponseDto>>(JSONString);
                resultResponse = new RestAPIGenericResponseDTO<CustomerResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, responseData);
            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<CustomerResponseDto>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<CustomerResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public async Task<RestAPIGenericResponseDTO<CustomerResponseDto>> GetById(int id, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<CustomerResponseDto>();

            try
            {
                var data = await customerService.GetByIdAsync(id);

                if (data != null)
                {
                    var customerResponse = await modelConverterService.ConvertToResponseDto<CustomerResponseDto>(data);
                    resultResponse = new RestAPIGenericResponseDTO<CustomerResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, customerResponse);
                }                    
                else
                    resultResponse = new RestAPIGenericResponseDTO<CustomerResponseDto>().WithSuccess(Constants.VALIDATION_ERROR_CODE, Constants.Customer_NOT_AVAILABE);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<CustomerResponseDto>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<CustomerResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public async Task<int> GetCountAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                return await customerService.GetCountAsync(generalFilter);

            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public async Task<RestAPIGenericResponseDTO<object>> UpdateAsync(CustomerRequestDto request, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
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
                var data = await customerService.GetByIdAsync(request.Id);
                if (data == null)
                    return new RestAPIGenericResponseDTO<object>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.Customer_NOT_AVAILABE);

                var entity = await modelConverterService.ConvertToEntityModel<Customer>(request);
                var updateProduct = await customerService.UpdateAsync(request.Id, entity);

                await auditLogService.UpdateActionAsync(profileInfo, data, entity);
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
                var data = await customerService.GetByIdAsync(id);
                if (data == null)
                    return new RestAPIGenericResponseDTO<object>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.Customer_NOT_AVAILABE);

                var updateStatus = await customerService.UpdateStatusAsync(id, status);

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
