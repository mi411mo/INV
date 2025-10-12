using Application.Interfaces.IBusinessLogic;
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
    public class GetMerchantByIdHandler : BaseHandler<GetMerchantByIdHandler>
    {
        private IMerchantBusinessService service;
        private readonly ILogger<GetMerchantByIdHandler> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        private const string ErrorMsg = "البيانات المدخلة غير صحيحة";
        private const string InternalErrorMsg = "خطأ داخلي";
        private int id;
        private readonly AuthorizationHelper _authorizationHelper;

        public GetMerchantByIdHandler(ILogger<GetMerchantByIdHandler> logger, IHttpClientFactory _httpClientFactory, IHttpContextAccessor accessor, IConfiguration config, AuthorizationHelper _authorizationHelper) : base(logger, _httpClientFactory, accessor, config, _authorizationHelper)
        {
            this.logger = logger;
            this.httpContextAccessor = accessor;
            this._authorizationHelper = _authorizationHelper;

        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                //var authResponse = await _authorizationHelper.ValidateUserPermissionsAsync(Policy.Invoicing.DetailsMerchants);

                //if (authResponse != null)
                //{
                //    return authResponse;
                //}
                service = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IMerchantBusinessService>();

                if (request.RequestUri.Segments.Length == 4)
                {
                    int.TryParse(request.RequestUri.Segments[3].Replace("/", ""), out int ccode);
                    id = ccode; // set merchant id
                }

                var serviceResponse = await service.GetById(id, base.SendAsync, request, cancellationToken);
                var responseTxt = JSONHelper<RestAPIGenericResponseDTO<MerchantResponseDto>>.GetJSONStr(serviceResponse);
                HttpStatusCode statusCode = serviceResponse.ResponseCode == Constants.THARWAT_SUCCESS_CODE ? HttpStatusCode.OK : HttpStatusCode.BadRequest;

                return PrepareResponse(responseTxt, statusCode);
            }
            catch(Exception ex)
            {
                throw new NotImplementedException();
            }           

        }

        HttpResponseMessage ResponseMessage(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                var msg = new RestAPIGenericResponseDTO<object>().WithError(response.StatusCode, ErrorMsg);
                response.Content = new StringContent(JsonConvert.SerializeObject(msg), Encoding.UTF8, "application/json");
                return response;
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                var msg = new RestAPIGenericResponseDTO<object>().WithError(response.StatusCode, InternalErrorMsg);
                response.Content = new StringContent(JsonConvert.SerializeObject(msg), Encoding.UTF8, "application/json");
                return response;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var msg = new RestAPIGenericResponseDTO<object>().WithError(response.StatusCode, ErrorMsg);
                response.Content = new StringContent(JsonConvert.SerializeObject(msg), Encoding.UTF8, "application/json");
                return response;
            }
            else
            {
                return response;
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
