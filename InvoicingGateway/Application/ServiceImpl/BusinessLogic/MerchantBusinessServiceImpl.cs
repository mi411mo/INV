using Application.Interfaces.IBusinessIndependentService.IServices;
using Application.Interfaces.IBusinessLogic;
using Application.IServices;
using Application.Utils.ModelConverter;
using Domain.Entities;
using Domain.Models;
using Domain.Models.Base;
using Domain.Models.Merchants;
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
    public class MerchantBusinessServiceImpl : IMerchantBusinessService
    {
        private readonly IMerchantService merchantService;
        private readonly ILogger<MerchantBusinessServiceImpl> logger;
        private readonly IAPIModelConverterService modelConverterService;
        private readonly IAuditLogService<Merchant> auditLogService;

        public MerchantBusinessServiceImpl(IMerchantService merchantService, ILogger<MerchantBusinessServiceImpl> logger, IAPIModelConverterService modelConverterService, IAuditLogService<Merchant> auditLogService)
        {
            this.merchantService = merchantService;
            this.logger = logger;
            this.modelConverterService = modelConverterService;
            this.auditLogService = auditLogService;
        }

        public async Task<RestAPIGenericResponseDTO<object>> ChangeStatusAsync(int id, int status, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<RestAPIGenericResponseDTO<object>> DeleteAsync(int id, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<object>();

            try
            {
                var data = await merchantService.GetByIdAsync(id);
                if (data == null)
                    return new RestAPIGenericResponseDTO<object>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.MERCHANT_NOT_AVAILABE);

                var deleteCustomer = await merchantService.DeleteAsync(id);

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

        public async Task<RestAPIGenericResponseDTO<MerchantResponseDto>> ExecuteAsync(MerchantRequestDto requestDto, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<MerchantResponseDto>();

            try
            {
                logger.LogInformation("Merchant Request DTO {}", requestDto.ToString());

                var validationErr = requestDto.IsValid();
                if (!string.IsNullOrEmpty(validationErr))
                {
                    // abort request processing & return error message
                    logger.LogInformation("Validation Error {}" + validationErr);
                    return new RestAPIGenericResponseDTO<MerchantResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, validationErr);
                }

                var entity = await modelConverterService.ConvertToEntityModel<Merchant>(requestDto);
                var merchantData = await merchantService.AddAsync(entity);
                if (merchantData.ReturnStatus == ReturnStatusEnum.Success)
                {
                    await auditLogService.CreateActionAsync(profileInfo, entity);
                    resultResponse = new RestAPIGenericResponseDTO<MerchantResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message);
                }
                else
                    resultResponse = new RestAPIGenericResponseDTO<MerchantResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, merchantData.Message);


            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<MerchantResponseDto>().WithSqlException(ex);
            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<MerchantResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public async Task<RestAPIGenericResponseDTO<MerchantResponseDto>> GetAll(GeneralFilterDto generalFilter, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<MerchantResponseDto>();

            try
            {
                var data = await merchantService.GetAllAsync(generalFilter);
                /* var ignore = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                 var JSONString = JsonConvert.SerializeObject(data, ignore);
                 var responseData = JsonConvert.DeserializeObject<IList<MerchantResponseDto>>(JSONString);*/
                var responseData = await modelConverterService.ConvertToListResponseDto<MerchantResponseDto, Merchant>(data);
                resultResponse = new RestAPIGenericResponseDTO<MerchantResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, responseData);
            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<MerchantResponseDto>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<MerchantResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public async Task<RestAPIGenericResponseDTO<MerchantResponseDto>> GetById(int id, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<MerchantResponseDto>();

            try
            {
                var data = await merchantService.GetByIdAsync(id);

                if (data != null)
                {
                    var merchantResponse = await modelConverterService.ConvertToResponseDto<MerchantResponseDto>(data);
                    resultResponse = new RestAPIGenericResponseDTO<MerchantResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, merchantResponse);
                }
                    
                else
                    resultResponse = new RestAPIGenericResponseDTO<MerchantResponseDto>().WithSuccess(Constants.VALIDATION_ERROR_CODE, Constants.MERCHANT_NOT_AVAILABE);

            }
            catch (SqlException ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<MerchantResponseDto>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                resultResponse = new RestAPIGenericResponseDTO<MerchantResponseDto>().WithException(ex);
            }

            return resultResponse;
        }

        public async Task<int> GetCountAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                return await merchantService.GetCountAsync(generalFilter);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<RestAPIGenericResponseDTO<object>> UpdateAsync(MerchantRequestDto requestDto, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            var resultResponse = new RestAPIGenericResponseDTO<object>();

            try
            {
                var validationErr = requestDto.IsValid();
                if (!string.IsNullOrEmpty(validationErr))
                {
                    // abort request processing & return error message
                    logger.LogInformation("Validation Error {}" + validationErr);
                    return new RestAPIGenericResponseDTO<object>().WithError(Constants.VALIDATION_ERROR_CODE, validationErr);
                }
                var data = await merchantService.GetByIdAsync(requestDto.Id);
                if (data == null)
                    return new RestAPIGenericResponseDTO<object>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.MERCHANT_NOT_AVAILABE);

                var entity = await modelConverterService.ConvertToEntityModel<Merchant>(requestDto);
                var updateProduct = await merchantService.UpdateAsync(requestDto.Id, entity);

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
                var data = await merchantService.GetByIdAsync(id);
                if (data == null)
                    return new RestAPIGenericResponseDTO<object>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.MERCHANT_NOT_AVAILABE);

                var updateStatus = await merchantService.UpdateStatusAsync(id, status);

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
