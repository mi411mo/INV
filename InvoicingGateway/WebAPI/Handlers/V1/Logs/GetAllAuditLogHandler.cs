using Application.Interfaces.IBusinessIndependentService.IServices;
using Application.Utils;
using Domain.Entities;
using Domain.Models;
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

namespace WebAPI.Handlers.V1.Logs
{
    public class GetAllAuditLogHandler : BaseHandler<GetAllAuditLogHandler>
    {
        private IAuditLogService<AuditLog> service;
        private readonly ILogger<GetAllAuditLogHandler> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        private const string ErrorMsg = "البيانات المدخلة غير صحيحة";
        private const string InternalErrorMsg = "خطأ داخلي";
        private readonly AuthorizationHelper _authorizationHelper;



        public GetAllAuditLogHandler(ILogger<GetAllAuditLogHandler> logger, IHttpClientFactory _httpClientFactory, IHttpContextAccessor accessor, IConfiguration config, AuthorizationHelper _authorizationHelper) : base(logger, _httpClientFactory, accessor, config, _authorizationHelper)
        {
            this.logger = logger;
            this.httpContextAccessor = accessor;
            this._authorizationHelper = _authorizationHelper;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //TODO:Add a new Policy for View Audit logs
            var response = await _authorizationHelper.ValidateUserPermissionsAsync(Policy.Invoicing.ViewCustomers + "||" + Policy.Invoicing.InfoCustomers);

            if (response != null)
            {
                return response;
            }
            service = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IAuditLogService<AuditLog>>();

            var generalFilter = await URLDecoder.DecodeURL(request);
            generalFilter.CustomerId = _authorizationHelper.GetUserAuditData().CustomerId;

            var logsList = await service.GetAllAsync(generalFilter);
            var serviceResponse = new RestAPIGenericResponseDTO<AuditLog>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, logsList);
            var responseTxt = JSONHelper<RestAPIGenericResponseDTO<AuditLog>>.GetJSONStr(serviceResponse);
            var totalRecords = await service.GetCountAsync(generalFilter);
            HttpStatusCode statusCode = serviceResponse.ResponseCode == Constants.THARWAT_SUCCESS_CODE ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return PrepareResponse(responseTxt, statusCode, generalFilter.PageNumber, generalFilter.PageSize, totalRecords);
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
