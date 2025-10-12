using Application.Interfaces.IBusinessLogic;
using Domain.Models;
using Domain.Models.Orders.ResponseDto;
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

namespace WebAPI.Handlers.V1.Invoices
{
    public class ConfirmInvoiceHandler : BaseHandler<ConfirmInvoiceHandler>
    {
        private IInvoiceBusinessService service;
        private readonly ILogger<ConfirmInvoiceHandler> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        private const string ErrorMsg = "البيانات المدخلة غير صحيحة";
        private const string InternalErrorMsg = "خطأ داخلي";
        private readonly AuthorizationHelper _authorizationHelper;

        public ConfirmInvoiceHandler(ILogger<ConfirmInvoiceHandler> logger, IHttpClientFactory _httpClientFactory, IHttpContextAccessor accessor, IConfiguration config, AuthorizationHelper _authorizationHelper) : base(logger, _httpClientFactory, accessor, config, _authorizationHelper)
        {
            this.logger = logger;
            this.httpContextAccessor = accessor;
            this._authorizationHelper = _authorizationHelper;

        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                /*var authResponse = await _authorizationHelper.ValidateUserPermissionsAsync(Policy.Invoicing.ManageInvoicesCategories,Constants.CONFIRM_INVOICE_SCOPE);

                if (authResponse.StatusCode != HttpStatusCode.OK)
                {
                    return authResponse;
                }*/
                service = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IInvoiceBusinessService>();

                if (request.RequestUri.Segments.Length == 5)
                {
                    string invRef = request.RequestUri.Segments[4].Replace("/", "");
                    var serviceResponse = await service.ConfirmInvoiceAsync(invRef, base.SendAsync, request, cancellationToken);

                    var responseTxt = JSONHelper<RestAPIGenericResponseDTO<InvoiceConfirmResponseDto>>.GetJSONStr(serviceResponse);
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
