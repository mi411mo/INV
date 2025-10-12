using Application.Interfaces.IBusinessIndependentService.IServices;
using Application.Interfaces.IBusinessLogic;
using Application.Utils;
using Domain.Entities;
using Domain.Models;
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

namespace WebAPI.Handlers.V1.Payments
{
    public class DirectPaymentHandler : BaseHandler<DirectPaymentHandler>
    {
        private const string SERVICE_NAME = "Direct Payment";
        private IPaymentBusinessService service;
        private ILogService logService;
        private readonly ILogger<DirectPaymentHandler> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly InputValidationService inputValidationService;
        private readonly AuthorizationHelper _authorizationHelper;

        public DirectPaymentHandler(InputValidationService inputValidationService, ILogger<DirectPaymentHandler> logger, IHttpClientFactory _httpClientFactory, IHttpContextAccessor accessor, IConfiguration config, AuthorizationHelper _authorizationHelper) : base(logger, _httpClientFactory, accessor, config, _authorizationHelper)
        {
            this.logger = logger;
            this.httpContextAccessor = accessor;
            this.inputValidationService = inputValidationService;
            this._authorizationHelper = _authorizationHelper;

        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                string profileId = string.Empty;
                string clientId = string.Empty;
                if (request.Headers.TryGetValues(Constants.PROFILE_ID_HEADER, out IEnumerable<string> profileHttpHeaders))
                    profileId = profileHttpHeaders.First();
                if (request.Headers.TryGetValues(Constants.CLIENT_ID_HEADER, out IEnumerable<string> clientHttpHeaders))
                    clientId = clientHttpHeaders.First();
                ProfileInfo profileInfo = new ProfileInfo(profileId, clientId);

                service = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IPaymentBusinessService>();
                logService = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<ILogService>();

                var jSONstring = await request.Content.ReadAsStringAsync();
                var jSONValidationResponse = await inputValidationService.Validate<DirectPaymentRequestDto>(jSONstring);
                if (jSONValidationResponse != string.Empty)
                {
                    logger.LogError("Error in JSON Input Validation, reason {}", jSONValidationResponse);

                    return PrepareResponse(jSONValidationResponse, HttpStatusCode.BadRequest);
                }

                ReshapeRequest(request);
                DirectPaymentRequestDto serviceRequest = await HTTPHelper<DirectPaymentRequestDto>.GetIsntance().GetBodyModel(request);

                serviceRequest.RequestId = Guid.NewGuid().ToString();
                //Save Log of the request
                var log1S = await logService.AddAsync(new Log()
                {
                    RequestId = serviceRequest.RequestId,
                    LogType = SERVICE_NAME,
                    SourceId = profileInfo.ClientId,
                    TargetId = Constants.FATORA_PLATFORM,
                    ServiceId = SERVICE_NAME,
                    Content = serviceRequest.ToString(),
                    Details = "Success Request! Start the Processing Logic Pipline",
                    ClientId = profileInfo.ClientId,
                    CustomerId = profileInfo.ProfileId
                });

                if (log1S.ReturnStatus != ReturnStatusEnum.Success)
                    logger.LogWarning("could not save request log record after successful input validation, reason {}", log1S.Message);


                var serviceResponse = await service.DirectPaymentAsync(serviceRequest, profileInfo, base.SendAsync, request, cancellationToken);

                var responseTxt = JSONHelper<RestAPIGenericResponseDTO<DirectPaymentResponseDto>>.GetJSONStr(serviceResponse);
                HttpStatusCode statusCode = (serviceResponse.ResponseCode == Constants.THARWAT_SUCCESS_CODE || serviceResponse.ResponseCode == Constants.FATORA_PENDING_CODE) ? HttpStatusCode.OK : HttpStatusCode.BadRequest;

                return PrepareResponse(responseTxt, statusCode);
            }
            catch(Exception ex)
            {
                var serviceResponse = new RestAPIGenericResponseDTO<DirectPaymentResponseDto>().WithError(Constants.SERVER_ERROR_CODE, Constants.SERVER_ERROR_MSG);
                var responseTxt = JSONHelper<RestAPIGenericResponseDTO<DirectPaymentResponseDto>>.GetJSONStr(serviceResponse);
                HttpStatusCode statusCode = serviceResponse.ResponseCode == Constants.THARWAT_SUCCESS_CODE ? HttpStatusCode.OK : HttpStatusCode.BadRequest;

                return PrepareResponse(responseTxt, statusCode);
            }
           
        }

        private async void ReshapeRequest(HttpRequestMessage request)
        {
            // read request body
            var reqString = await request.Content.ReadAsStringAsync();
            var requestDto = JsonConvert.DeserializeObject<DirectPaymentRequestDto>(reqString);

            // convert request body model to JSON string
            var clientRequestJson = JsonConvert.SerializeObject(requestDto);
            // create new HttpContent with the modified body
            var requestContent = new StringContent(clientRequestJson, Encoding.UTF8, "application/json");

            // reflect changes to original request body
            request.Content = requestContent;

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
