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
    public class DeleteCategoryHandler : BaseHandler<DeleteCategoryHandler>
    {
        private ICategoryBusinessService service;
        private readonly ILogger<DeleteCategoryHandler> logger;
        private readonly IHttpContextAccessor accessor;
        private int id;
        private readonly AuthorizationHelper _authorizationHelper;

        public DeleteCategoryHandler(ILogger<DeleteCategoryHandler> logger, IHttpClientFactory _httpClientFactory, IHttpContextAccessor accessor, IConfiguration config, AuthorizationHelper _authorizationHelper) : base(logger, _httpClientFactory, accessor, config, _authorizationHelper)
        {
            this.logger = logger;
            this.accessor = accessor;
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

            if (request.RequestUri.Segments.Length == 4)
            {
                int.TryParse(request.RequestUri.Segments[3].Replace("/", ""), out int ccode);
                id = ccode; // set merchant id
            }
        
            ProfileInfo profileInfo = new ProfileInfo(_authorizationHelper.GetUserAuditData().CustomerId, _authorizationHelper.GetUserAuditData().ClientId, _authorizationHelper.GetUserAuditData().UserId);

            var serviceResponse = await service.DeleteAsync(id, profileInfo, base.SendAsync, request, cancellationToken);
            var responseTxt = JSONHelper<RestAPIGenericResponseDTO<object>>.GetJSONStr(serviceResponse);
            HttpStatusCode statusCode = serviceResponse.ResponseCode == Constants.THARWAT_SUCCESS_CODE ? HttpStatusCode.OK : HttpStatusCode.BadRequest;

            return PrepareResponse(responseTxt, statusCode);
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
