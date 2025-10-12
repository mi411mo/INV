using Application.Interfaces.IBusinessIndependentService.IServices;
using Application.Interfaces.IBusinessLogic;
using Application.Utils;
using Domain.Models;
using Domain.Models.Categories;
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

namespace WebAPI.Handlers.V1.Categories
{
    public class CreateCategoryHandler : BaseHandler<CreateCategoryHandler>
    {
        private ICategoryBusinessService service;
        private readonly ILogger<CreateCategoryHandler> logger;
        private readonly IHttpContextAccessor accessor;
        private readonly InputValidationService inputValidationService;
        private const string ErrorMsg = "البيانات المدخلة غير صحيحة";
        private const string InternalErrorMsg = "خطأ داخلي";
        private readonly AuthorizationHelper _authorizationHelper;


        public CreateCategoryHandler(InputValidationService inputValidationService, ILogger<CreateCategoryHandler> logger, IHttpClientFactory _httpClientFactory, IHttpContextAccessor accessor, IConfiguration config, AuthorizationHelper _authorizationHelper) : base(logger, _httpClientFactory, accessor, config, _authorizationHelper)
        {
            this.logger = logger;
            this.accessor = accessor;
            this.inputValidationService = inputValidationService;
            this._authorizationHelper = _authorizationHelper;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
         
            var response = await _authorizationHelper.ValidateUserPermissionsAsync(Policy.Invoicing.ManageInvoicesCategories);

            if (response != null) 
            {
                return response;
            }
            service = accessor.HttpContext.RequestServices.GetRequiredService<ICategoryBusinessService>();
            var jSONstring = await request.Content.ReadAsStringAsync();
            var jSONValidationResponse = await inputValidationService.Validate<CategoryRequestDto>(jSONstring);
            if (jSONValidationResponse != string.Empty)
            {
                logger.LogError("Error in JSON Input Validation, reason {}", jSONValidationResponse);
                return PrepareResponse(jSONValidationResponse, HttpStatusCode.BadRequest);
            }

            ReshapeRequest(request);

            CategoryRequestDto serviceRequest = await HTTPHelper<CategoryRequestDto>.GetIsntance().GetBodyModel(request);
            serviceRequest.CustomerId = _authorizationHelper.GetUserAuditData().CustomerId;
            serviceRequest.ClientId = _authorizationHelper.GetUserAuditData().ClientId;
            serviceRequest.UserId = _authorizationHelper.GetUserAuditData().UserId;

            ProfileInfo profileInfo = new ProfileInfo(serviceRequest.CustomerId, serviceRequest.ClientId, serviceRequest.UserId);

            var serviceResponse = await service.ExecuteAsync(serviceRequest, profileInfo, base.SendAsync, request, cancellationToken);
            var responseTxt = JSONHelper<RestAPIGenericResponseDTO<CategoryResponseDto>>.GetJSONStr(serviceResponse);
            HttpStatusCode statusCode = serviceResponse.ResponseCode == Constants.THARWAT_SUCCESS_CODE ? HttpStatusCode.OK : HttpStatusCode.BadRequest;

            return PrepareResponse(responseTxt, statusCode);
        }

        private async void ReshapeRequest(HttpRequestMessage request)
        {
            // read request body
            var reqString = await request.Content.ReadAsStringAsync();
            var serializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore, // good practice
                MissingMemberHandling = MissingMemberHandling.Ignore,
                //DefaultValueHandling = DefaultValueHandling.Ignore // Not needed with Required.Default
            };
            var requestDto = JsonConvert.DeserializeObject<CategoryRequestDto>(reqString, serializerSettings);

            // Null check in case deserialization fails for any other reason
            if (requestDto == null)
            {
                //  Log the error, return an error response, etc.
                //Handle deserialization failure
                return;
            }
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
