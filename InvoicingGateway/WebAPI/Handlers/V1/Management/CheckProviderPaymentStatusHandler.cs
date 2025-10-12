using Application.Interfaces.IBusinessIndependentService.IServices;
using Application.Interfaces.IBusinessLogic;
using Application.Utils;
using Domain.Entities;
using Domain.Models;
using Domain.Models.Orders.ResponseDto;
using Domain.Models.Payments.RequestDto;
using Domain.Models.Payments.ResponseDto;
using Domain.Utils;
using Domain.Utils.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tharawat.Switch.Policy;
using Tharwat.Invoices.Gateway.Handlers.V1.Base;
using WebAPI.Utils;

namespace WebAPI.Handlers.V1.Management
{
    public class CheckProviderPaymentStatusHandler : BaseHandler<CheckProviderPaymentStatusHandler>
    {
        private const string SERVICE_NAME = "Check Provider Payment Status";
        private IPaymentBusinessService service;
        private ILogService logService;
        private readonly ILogger<CheckProviderPaymentStatusHandler> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly AuthorizationHelper _authorizationHelper;


        public CheckProviderPaymentStatusHandler(ILogger<CheckProviderPaymentStatusHandler> logger, IHttpClientFactory _httpClientFactory, IHttpContextAccessor accessor, IConfiguration config, AuthorizationHelper _authorizationHelper) : base(logger, _httpClientFactory, accessor, config, _authorizationHelper)
        {
            this.logger = logger;
            this.httpContextAccessor = accessor;
            this._authorizationHelper = _authorizationHelper;

        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {

                var authResponse = await _authorizationHelper.ValidateUserPermissionsAsync(Policy.Invoicing.InfoPayments + "||" + Policy.Invoicing.ViewPayments);

                if (authResponse != null)
                {
                    return authResponse;
                }

                service = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IPaymentBusinessService>();
                logService = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<ILogService>();

                var requestId = Guid.NewGuid().ToString();

                var log1S = await logService.AddAsync(new Log()
                {
                    RequestId = requestId,
                    LogType = SERVICE_NAME,
                    SourceId = _authorizationHelper.GetUserAuditData().UserId,
                    TargetId = Constants.FATORA_PLATFORM,
                    ServiceId = SERVICE_NAME,
                    Content = request.RequestUri.Segments.ToString(),
                    Details = "Success Request! Start the Processing Logic Pipline",
                    ClientId = _authorizationHelper.GetUserAuditData().ClientId,
                    CustomerId = _authorizationHelper.GetUserAuditData().CustomerId
                });
                if (log1S.ReturnStatus != ReturnStatusEnum.Success)
                    logger.LogWarning("could not save request log record after successful input validation, reason {}", log1S.Message);

                if (request.RequestUri.Segments.Length == 5)
                {
                    string payRef = request.RequestUri.Segments[4].Replace("/", "");
                   
                    var serviceResponse = await service.CheckProviderPaymentStatus(payRef, base.SendAsync, request, cancellationToken);
                    var responseTxt = JSONHelper<RestAPIGenericResponseDTO<CheckProviderPaymentStatusResponseDto>>.GetJSONStr(serviceResponse);

                    //Save Log of the response
                    log1S = await logService.AddAsync(new Log()
                    {
                        RequestId = requestId,
                        LogType = SERVICE_NAME,
                        SourceId = _authorizationHelper.GetUserAuditData().UserId,
                        TargetId = Constants.FATORA_PLATFORM,
                        ServiceId = SERVICE_NAME,
                        Content = responseTxt,
                        Details = "transaction has been processed successfully",
                        ClientId = _authorizationHelper.GetUserAuditData().ClientId,
                        CustomerId = _authorizationHelper.GetUserAuditData().CustomerId
                    });
                    HttpStatusCode statusCode = serviceResponse.ResponseCode == Constants.THARWAT_SUCCESS_CODE ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
                    return PrepareResponse(responseTxt, statusCode);
                }
                else
                {
                    var response = new RestAPIGenericResponseDTO<CheckProviderPaymentStatusResponseDto>().WithError(Constants.CLIENT_ERROR_CODE, "Request URL is invalid");
                    var responseTxt = JSONHelper<RestAPIGenericResponseDTO<CheckProviderPaymentStatusResponseDto>>.GetJSONStr(response);
                    return PrepareResponse(responseTxt, HttpStatusCode.BadRequest);
                }


            }
            catch (Exception ex)
            {
                logger.LogWarning("Throw an exception, reason {}", ex.Message);
                var response = new RestAPIGenericResponseDTO<CheckProviderPaymentStatusResponseDto>().WithException(ex);
                var responseTxt = JSONHelper<RestAPIGenericResponseDTO<CheckProviderPaymentStatusResponseDto>>.GetJSONStr(response);
                return PrepareResponse(responseTxt, HttpStatusCode.InternalServerError);
            }

        }
      

        private HttpResponseMessage PrepareResponse(string responseText, HttpStatusCode responseCode)
        {
            return new HttpResponseMessage(responseCode)
            {
                Content = new StringContent(responseText, Encoding.UTF8, "application/json")
            };
        }
    }
}
