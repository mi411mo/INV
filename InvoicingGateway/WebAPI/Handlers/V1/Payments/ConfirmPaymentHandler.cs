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

namespace WebAPI.Handlers.V1.Payments
{
    public class ConfirmPaymentHandler : BaseHandler<ConfirmPaymentHandler>
    {
        private const string SERVICE_NAME = "Confirm Payment";
        private IPaymentBusinessService service;
        private ILogService logService;
        private readonly ILogger<ConfirmPaymentHandler> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        private const string ErrorMsg = "البيانات المدخلة غير صحيحة";
        private const string InternalErrorMsg = "خطأ داخلي";
        private readonly AuthorizationHelper _authorizationHelper;


        public ConfirmPaymentHandler(ILogger<ConfirmPaymentHandler> logger, IHttpClientFactory _httpClientFactory, IHttpContextAccessor accessor, IConfiguration config, AuthorizationHelper _authorizationHelper) : base(logger, _httpClientFactory, accessor, config, _authorizationHelper)
        {
            this.logger = logger;
            this.httpContextAccessor = accessor;
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
                
                if (request.RequestUri.Segments.Length == 5)
                {
                    string payRef = request.RequestUri.Segments[4].Replace("/", "");
                    ReshapeRequest(request);
                    ConfirmPaymentRequestDto serviceRequest = await HTTPHelper<ConfirmPaymentRequestDto>.GetIsntance().GetBodyModel(request);
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

                    var serviceResponse = await service.ConfirmPaymentAsync(payRef, serviceRequest, profileInfo, base.SendAsync, request, cancellationToken);
                    var responseTxt = JSONHelper<RestAPIGenericResponseDTO<ConfirmPaymentResponseDto>>.GetJSONStr(serviceResponse);

                    //Save Log of the response
                    log1S = await logService.AddAsync(new Log()
                    {
                        RequestId = serviceRequest.RequestId,
                        LogType = SERVICE_NAME,
                        SourceId = profileInfo.ClientId,
                        TargetId = Constants.FATORA_PLATFORM,
                        ServiceId = SERVICE_NAME,
                        Content = responseTxt,
                        Details = "transaction has been processed successfully",
                        ClientId = profileInfo.ClientId,
                        CustomerId = profileInfo.ProfileId
                    });
                    HttpStatusCode statusCode = serviceResponse.ResponseCode == Constants.THARWAT_SUCCESS_CODE ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
                    return PrepareResponse(responseTxt, statusCode);
                }
                else
                {
                    var response = new RestAPIGenericResponseDTO<InvoiceConfirmResponseDto>().WithError(Constants.CLIENT_ERROR_CODE, "Request URL is invalid");
                    var responseTxt = JSONHelper<RestAPIGenericResponseDTO<InvoiceConfirmResponseDto>>.GetJSONStr(response);
                    return PrepareResponse(responseTxt, HttpStatusCode.BadRequest);
                }


            }
            catch (Exception ex)
            {
                logger.LogWarning("Throw an exception, reason {}", ex.Message);
                var response = new RestAPIGenericResponseDTO<InvoiceConfirmResponseDto>().WithException(ex);
                var responseTxt = JSONHelper<RestAPIGenericResponseDTO<InvoiceConfirmResponseDto>>.GetJSONStr(response);
                return PrepareResponse(responseTxt, HttpStatusCode.InternalServerError);
            }

        }

        private async void ReshapeRequest(HttpRequestMessage request)
        {
            // read request body
            var reqString = await request.Content.ReadAsStringAsync();
            var requestDto = JsonConvert.DeserializeObject<ConfirmPaymentRequestDto>(reqString);

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
