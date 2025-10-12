using Application.Interfaces.IBusinessIndependentService.IServices;
using Application.Interfaces.IBusinessLogic;
using Application.Utils.ModelConverter;
using Domain.Entities;
using Domain.Models;
using Domain.Models.AuditLogs;
using Domain.Models.Base;
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
    public class AuditLogBusinessServiceImpl : IAuditLogBusinessService
    {
        private readonly IAuditLogService<AuditLog> auditLogService;
        private readonly ILogger<CategoryBusinessServiceImpl> logger;
        private readonly IAPIModelConverterService modelConverterService;

        public AuditLogBusinessServiceImpl(IAuditLogService<AuditLog> auditLogService, ILogger<CategoryBusinessServiceImpl> logger, IAPIModelConverterService modelConverterService)
        {
            this.auditLogService = auditLogService;
            this.logger = logger;
            this.modelConverterService = modelConverterService;
        }
        public async Task<RestAPIGenericResponseDTO<AuditLogResponseDto>> GetAll(GeneralFilterDto generalFilter, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {

            try
            {
                var data = await auditLogService.GetAllAsync(generalFilter);
                var ignore = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                var JSONString = JsonConvert.SerializeObject(data, ignore);
                var responseData = JsonConvert.DeserializeObject<IList<AuditLogResponseDto>>(JSONString);
                return new RestAPIGenericResponseDTO<AuditLogResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, responseData);
            }
            catch (SqlException ex)
            {
                return new RestAPIGenericResponseDTO<AuditLogResponseDto>().WithSqlException(ex);

            }
            catch (Exception ex)
            {
                return new RestAPIGenericResponseDTO<AuditLogResponseDto>().WithException(ex);
            }
        }
    }
}
