using Application.Interfaces.IBusinessLogic;
using Application.Utils;
using Domain.Models;
using Domain.Models.Merchants;
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

namespace WebAPI.Handlers.V1.Merchants
{
    public class UpdateMerchantHandler : BaseHandler<UpdateMerchantHandler>
    {
        private IMerchantBusinessService service;
        private readonly ILogger<UpdateMerchantHandler> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        private const string ErrorMsg = "البيانات المدخلة غير صحيحة";
        private const string InternalErrorMsg = "خطأ داخلي";
        private readonly AuthorizationHelper _authorizationHelper;

        public UpdateMerchantHandler(ILogger<UpdateMerchantHandler> logger, IHttpClientFactory _httpClientFactory, IHttpContextAccessor accessor, IConfiguration config, AuthorizationHelper _authorizationHelper) : base(logger, _httpClientFactory, accessor, config, _authorizationHelper)
        {
            this.logger = logger;
            this.httpContextAccessor = accessor;
            this._authorizationHelper = _authorizationHelper;

        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authResponse = await _authorizationHelper.ValidateUserPermissionsAsync(Policy.Invoicing.ManageMerchants);

            if (authResponse != null)
            {
                return authResponse;
            }
            service = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IMerchantBusinessService>();

            ReshapeRequest(request);

            MerchantRequestDto serviceRequest = await HTTPHelper<MerchantRequestDto>.GetIsntance().GetBodyModel(request);

            ProfileInfo profileInfo = new ProfileInfo(_authorizationHelper.GetUserAuditData().CustomerId, _authorizationHelper.GetUserAuditData().ClientId, _authorizationHelper.GetUserAuditData().UserId);
            var serviceResponse = await service.UpdateAsync(serviceRequest, profileInfo, base.SendAsync, request, cancellationToken);

            var responseTxt = JSONHelper<RestAPIGenericResponseDTO<object>>.GetJSONStr(serviceResponse);
            HttpStatusCode statusCode = serviceResponse.ResponseCode == Constants.THARWAT_SUCCESS_CODE ? HttpStatusCode.OK : HttpStatusCode.BadRequest;

            return PrepareResponse(responseTxt, statusCode);
        }

        private async void ReshapeRequest(HttpRequestMessage request)
        {
            // read request body
            var reqString = await request.Content.ReadAsStringAsync();
            var requestDto = JsonConvert.DeserializeObject<MerchantRequestDto>(reqString);

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
