using Application.Interfaces.IBusinessLogic;
using Domain.Entities;
using Domain.Models;
using Domain.Models.Orders.ResponseDto;
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

namespace WebAPI.Handlers.V1.Orders
{
    public class GetOrderByReferenceHandler : BaseHandler<GetOrderByReferenceHandler>
    {
        private IOrderBusinessService service;
        private readonly ILogger<GetOrderByReferenceHandler> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        private const string ErrorMsg = "البيانات المدخلة غير صحيحة";
        private const string InternalErrorMsg = "خطأ داخلي";
        private string orderRef;
        private readonly AuthorizationHelper _authorizationHelper;

        public GetOrderByReferenceHandler(ILogger<GetOrderByReferenceHandler> logger, IHttpClientFactory _httpClientFactory, IHttpContextAccessor accessor, IConfiguration config, AuthorizationHelper _authorizationHelper) : base(logger, _httpClientFactory, accessor, config, _authorizationHelper)
        {
            this.logger = logger;
            this.httpContextAccessor = accessor;
            this._authorizationHelper = _authorizationHelper;

        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                //var authResponse = await _authorizationHelper.ValidateUserPermissionsAsync(Policy.Invoicing.DetailsOrders);

                //if (authResponse != null)
                //{
                //    return authResponse;
                //}
                service = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IOrderBusinessService>();

                if (request.RequestUri.Segments.Length == 5)
                {
                    orderRef = request.RequestUri.Segments[4].Replace("/", "");
                }

                var serviceResponse = await service.GetByRef(orderRef, base.SendAsync, request, cancellationToken);
                var responseTxt = JSONHelper<RestAPIGenericResponseDTO<GetOrderResponseDto>>.GetJSONStr(serviceResponse);
                HttpStatusCode statusCode = serviceResponse.ResponseCode == Constants.THARWAT_SUCCESS_CODE ? HttpStatusCode.OK : HttpStatusCode.BadRequest;

                return PrepareResponse(responseTxt, statusCode);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
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
