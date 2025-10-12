using Application.Interfaces.IBusinessLogic;
using Domain.Models;
using Domain.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
    public class DeleteCategoryTypeHandler : BaseHandler<DeleteCategoryTypeHandler>
    {
        private ICategoryTypeBusinessService service;
        private readonly ILogger<DeleteCategoryTypeHandler> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        private int id;
        private readonly AuthorizationHelper _authorizationHelper;

       

        public DeleteCategoryTypeHandler(ILogger<DeleteCategoryTypeHandler> logger, IHttpClientFactory _httpClientFactory, IHttpContextAccessor accessor, IConfiguration config, AuthorizationHelper _authorizationHelper) : base(logger, _httpClientFactory, accessor, config, _authorizationHelper)
        {
            this.logger = logger;
            this.httpContextAccessor = accessor;
            this._authorizationHelper = _authorizationHelper;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authResponse = await _authorizationHelper.ValidateUserPermissionsAsync(Policy.Invoicing.ManageCategoryTypes);

            if (authResponse != null)
            {
                return authResponse;
            }
            service = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<ICategoryTypeBusinessService>();

            if (request.RequestUri.Segments.Length == 4)
            {
                int.TryParse(request.RequestUri.Segments[3].Replace("/", ""), out int ccode);
                id = ccode; // set category type id

                ProfileInfo profileInfo = new ProfileInfo(_authorizationHelper.GetUserAuditData().CustomerId, _authorizationHelper.GetUserAuditData().ClientId, _authorizationHelper.GetUserAuditData().UserId);
                var serviceResponse = await service.DeleteAsync(id, profileInfo, base.SendAsync, request, cancellationToken);
                var responseTxt = JSONHelper<RestAPIGenericResponseDTO<object>>.GetJSONStr(serviceResponse);
                HttpStatusCode statusCode = serviceResponse.ResponseCode == Constants.THARWAT_SUCCESS_CODE ? HttpStatusCode.OK : HttpStatusCode.BadRequest;

                return PrepareResponse(responseTxt, statusCode);
            }
            else
            {
                var response = new RestAPIGenericResponseDTO<object>().WithError(Constants.CLIENT_ERROR_CODE, "Request URL is invalid");
                var responseTxt = JSONHelper<RestAPIGenericResponseDTO<object>>.GetJSONStr(response);
                return PrepareResponse(responseTxt, HttpStatusCode.BadRequest);
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
