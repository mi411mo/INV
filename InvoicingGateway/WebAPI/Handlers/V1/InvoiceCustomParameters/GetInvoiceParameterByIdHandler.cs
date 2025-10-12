using Application.Interfaces.IBusinessIndependentService.IServices;
using Application.Interfaces.IBusinessLogic;
using Domain.Models;
using Domain.Models.Categories;
using Domain.Models.InvoiceCustomParameters;
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

namespace WebAPI.Handlers.V1.InvoiceCustomParameters
{
    public class GetInvoiceParameterByIdHandler : BaseHandler<GetInvoiceParameterByIdHandler>
    {
        private IInvoiceParameterBusinessService service;
        private readonly ILogger<GetInvoiceParameterByIdHandler> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly InputValidationService inputValidationService;
        private const string ErrorMsg = "البيانات المدخلة غير صحيحة";
        private const string InternalErrorMsg = "خطأ داخلي";
        private readonly AuthorizationHelper _authorizationHelper;

        public GetInvoiceParameterByIdHandler(InputValidationService inputValidationService,ILogger<GetInvoiceParameterByIdHandler> logger, IHttpClientFactory _httpClientFactory, IHttpContextAccessor accessor, IConfiguration config, AuthorizationHelper _authorizationHelper) : base(logger, _httpClientFactory, accessor, config, _authorizationHelper)
        {
            this.logger = logger;
            this.httpContextAccessor = accessor;
            this.inputValidationService = inputValidationService;
            this._authorizationHelper = _authorizationHelper;

        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var authResponse = await _authorizationHelper.ValidateUserPermissionsAsync(Policy.Invoicing.DetailsInvoiceCustomParameters);

                if (authResponse != null)
                {
                    return authResponse;
                }
                service = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IInvoiceParameterBusinessService>();

                if (request.RequestUri.Segments.Length == 4)
                {
                    int.TryParse(request.RequestUri.Segments[3].Replace("/", ""), out int ccode);
                    int id = ccode; // set category id
                    var serviceResponse = await service.GetById(id, base.SendAsync, request, cancellationToken);
                    var responseTxt = JSONHelper<RestAPIGenericResponseDTO<InvoiceParametersResponseDto>>.GetJSONStr(serviceResponse);
                    HttpStatusCode statusCode = serviceResponse.ResponseCode == Constants.THARWAT_SUCCESS_CODE ? HttpStatusCode.OK : HttpStatusCode.BadRequest;

                    return PrepareResponse(responseTxt, statusCode);
                }
                else
                {
                    var response = new RestAPIGenericResponseDTO<InvoiceParametersResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, "Request URL is invalid");
                    var responseTxt = JSONHelper<RestAPIGenericResponseDTO<InvoiceParametersResponseDto>>.GetJSONStr(response);
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
