using Application.Interfaces.IBusinessLogic;
using Application.Utils;
using Domain.Models;
using Domain.Models.Payments.ResponseDto;
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

namespace WebAPI.Handlers.V1.Payments
{
    public class GetAllPaymentsHandler :  BaseHandler<GetAllPaymentsHandler>
    {
        private IPaymentBusinessService service;
        private readonly ILogger<GetAllPaymentsHandler> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        private const string ErrorMsg = "البيانات المدخلة غير صحيحة";
        private const string InternalErrorMsg = "خطأ داخلي";
        private readonly AuthorizationHelper _authorizationHelper;


        public GetAllPaymentsHandler(ILogger<GetAllPaymentsHandler> logger, IHttpClientFactory _httpClientFactory, IHttpContextAccessor accessor, IConfiguration config, AuthorizationHelper _authorizationHelper) : base(logger, _httpClientFactory, accessor, config, _authorizationHelper)
        {
            this.logger = logger;
            this.httpContextAccessor = accessor;
            this._authorizationHelper = _authorizationHelper;

        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authResponse = await _authorizationHelper.ValidateUserPermissionsAsync(Policy.Invoicing.InfoPayments + "||" + Policy.Invoicing.ViewPayments);

            if (authResponse != null)
            {
                return authResponse;
            }

            logger.LogInformation("Succeed Authentication");
            service = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IPaymentBusinessService>();
            var generalFilter = await URLDecoder.DecodeURL(request);
            generalFilter.CustomerId = _authorizationHelper.GetUserAuditData().CustomerId;

            var serviceResponse = await service.GetAll(generalFilter, base.SendAsync, request, cancellationToken);

            var responseTxt = JSONHelper<RestAPIGenericResponseDTO<PaymentResponseDto>>.GetJSONStr(serviceResponse);
            var totalRecords = await service.GetCountAsync(generalFilter);
            HttpStatusCode statusCode = serviceResponse.ResponseCode == Constants.THARWAT_SUCCESS_CODE ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return PrepareResponse(responseTxt, statusCode, generalFilter.PageNumber, generalFilter.PageSize, totalRecords);

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

        private HttpResponseMessage PrepareResponse(string responseText, HttpStatusCode responseCode, int pageNumber, int pageSize, int totalRecords)
        {
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            var metadata = new
            {
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                TotalRecords = totalRecords,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                HasPrevious = pageNumber > 1 ? true : false,
                HasNext = pageNumber < totalPages ? true : false

            };
            var response = new HttpResponseMessage(responseCode)
            {
                Content = new StringContent(responseText, Encoding.UTF8, "application/json")
            };
            response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return response;
        }
    }
}
