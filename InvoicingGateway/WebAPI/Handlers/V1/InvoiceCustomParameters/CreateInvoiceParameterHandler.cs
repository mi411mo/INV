using Application.Interfaces.IBusinessIndependentService.IServices;
using Application.Interfaces.IBusinessLogic;
using Application.Utils;
using Domain.Models;
using Domain.Models.Categories;
using Domain.Models.InvoiceCustomParameters;
using Domain.Utils;
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

namespace WebAPI.Handlers.V1.InvoiceCustomParameters
{
    public class CreateInvoiceParameterHandler : BaseHandler<CreateInvoiceParameterHandler>
    {
        private IInvoiceParameterBusinessService service;
        private readonly ILogger<CreateInvoiceParameterHandler> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly InputValidationService inputValidationService;
        private const string ErrorMsg = "البيانات المدخلة غير صحيحة";
        private const string InternalErrorMsg = "خطأ داخلي";
        private readonly AuthorizationHelper _authorizationHelper;

        public CreateInvoiceParameterHandler(InputValidationService inputValidationService,ILogger<CreateInvoiceParameterHandler> logger, IHttpClientFactory _httpClientFactory, IHttpContextAccessor accessor, IConfiguration config, AuthorizationHelper _authorizationHelper) : base(logger, _httpClientFactory, accessor, config, _authorizationHelper)
        {
            this.logger = logger;
            this.httpContextAccessor = accessor;
            this.inputValidationService = inputValidationService;
            this._authorizationHelper = _authorizationHelper;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await _authorizationHelper.ValidateUserPermissionsAsync(Policy.Invoicing.ManageInvoiceCustomParameters);

            if (response != null)
            {
                return response;
            }
            service = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IInvoiceParameterBusinessService>();
            var jSONstring = await request.Content.ReadAsStringAsync();
            var jSONValidationResponse = await inputValidationService.Validate<InvoiceParametersRequestDto>(jSONstring);
            if (jSONValidationResponse != string.Empty)
            {
                logger.LogError("Error in JSON Input Validation, reason {}", jSONValidationResponse);
                return PrepareResponse(jSONValidationResponse, HttpStatusCode.BadRequest);
            }

            ReshapeRequest(request);

            InvoiceParametersRequestDto serviceRequest = await HTTPHelper<InvoiceParametersRequestDto>.GetIsntance().GetBodyModel(request);
            serviceRequest.CustomerId = _authorizationHelper.GetUserAuditData().CustomerId;
            serviceRequest.ClientId = _authorizationHelper.GetUserAuditData().ClientId;
            serviceRequest.UserId = _authorizationHelper.GetUserAuditData().UserId;

            ProfileInfo profileInfo = new ProfileInfo(serviceRequest.CustomerId, serviceRequest.ClientId, serviceRequest.UserId);
            var serviceResponse = await service.ExecuteAsync(serviceRequest, profileInfo, base.SendAsync, request, cancellationToken);
            var responseTxt = JSONHelper<RestAPIGenericResponseDTO<InvoiceParametersResponseDto>>.GetJSONStr(serviceResponse);
            HttpStatusCode statusCode = serviceResponse.ResponseCode == Constants.THARWAT_SUCCESS_CODE ? HttpStatusCode.OK : HttpStatusCode.BadRequest;

            return PrepareResponse(responseTxt, statusCode);
        }

        private async void ReshapeRequest(HttpRequestMessage request)
        {
            // read request body
            var reqString = await request.Content.ReadAsStringAsync();
            var requestDto = JsonConvert.DeserializeObject<InvoiceParametersRequestDto>(reqString);
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
