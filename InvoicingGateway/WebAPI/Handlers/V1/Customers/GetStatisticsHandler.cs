using Application.Interfaces.IBusinessLogic;
using Application.Utils;
using Domain.Models;
using Domain.Models.Customers;
using Domain.Models.Statistics;
using Domain.Utils;
using Domain.Utils.Enums;
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

namespace WebAPI.Handlers.V1.Customers
{
    public class GetStatisticsHandler : BaseHandler<GetStatisticsHandler>
    {
        private ICustomerBusinessService customerService;
        private IProductBusinessService productService;
        private IMerchantBusinessService merchantService;
        private IInvoiceBusinessService invoiceService;
        private IOrderBusinessService orderService;
        private ICategoryBusinessService categoryService;
        private IPaymentBusinessService paymentService;
        private readonly ILogger<GetStatisticsHandler> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        private const string ErrorMsg = "البيانات المدخلة غير صحيحة";
        private const string InternalErrorMsg = "خطأ داخلي";
        private readonly AuthorizationHelper _authorizationHelper;

     

        public GetStatisticsHandler(ILogger<GetStatisticsHandler> logger, IHttpClientFactory _httpClientFactory, IHttpContextAccessor accessor, IConfiguration config, AuthorizationHelper _authorizationHelper) : base(logger, _httpClientFactory, accessor, config, _authorizationHelper)
        {
            this.logger = logger;
            this.httpContextAccessor = accessor;
            this._authorizationHelper = _authorizationHelper;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await _authorizationHelper.ValidateUserPermissionsAsync(Policy.Invoicing.DetailsProducts);

            if (response != null)
            {
                return response;
            }
            int totalRecords = 0;
            var generalFilter = await URLDecoder.DecodeURL(request);
            generalFilter.CustomerId = _authorizationHelper.GetUserAuditData().CustomerId;
            switch (generalFilter.EntityType)
            {
                case EntityTypeEnum.Customers:
                    {
                        customerService = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<ICustomerBusinessService>();
                        totalRecords = await customerService.GetCountAsync(generalFilter);
                        break;
                    }
                case EntityTypeEnum.Products:
                    {
                        productService = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IProductBusinessService>();
                        totalRecords = await productService.GetCountAsync(generalFilter);
                        break;
                    }
                case EntityTypeEnum.Invoices:
                    {
                        invoiceService = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IInvoiceBusinessService>();
                        totalRecords = await invoiceService.GetCountAsync(generalFilter);
                        break;
                    }
                case EntityTypeEnum.Merchants:
                    {
                        merchantService = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IMerchantBusinessService>();
                        totalRecords = await merchantService.GetCountAsync(generalFilter);
                        break;
                    }
                case EntityTypeEnum.Orders:
                    {
                        orderService = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IOrderBusinessService>();
                        totalRecords = await orderService.GetCountAsync(generalFilter);
                        break;
                    }
                case EntityTypeEnum.Categories:
                    {
                        categoryService = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<ICategoryBusinessService>();
                        totalRecords = await categoryService.GetCountAsync(generalFilter);
                        break;
                    }
                case EntityTypeEnum.Payments:
                    {
                        paymentService = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IPaymentBusinessService>();
                        totalRecords = await paymentService.GetCountAsync(generalFilter);
                        break;
                    }
                default:
                    {
                        var generalRes = new RestAPIGenericResponseDTO<StatisticsResponseDto>().WithError(Constants.VALIDATION_ERROR_CODE, Constants.ENTITY_TYPE_NOT_AVAILABE);
                        var resTxt = JSONHelper<RestAPIGenericResponseDTO<StatisticsResponseDto>>.GetJSONStr(generalRes);

                        HttpStatusCode statCode = generalRes.ResponseCode == Constants.THARWAT_SUCCESS_CODE ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
                        return PrepareResponse(resTxt, statCode);
                    }

            }

            var serviceResponse = new StatisticsResponseDto(totalRecords);
            var generalResponse = new RestAPIGenericResponseDTO<StatisticsResponseDto>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, serviceResponse);
            var responseTxt = JSONHelper<RestAPIGenericResponseDTO<StatisticsResponseDto>>.GetJSONStr(generalResponse);

            HttpStatusCode statusCode = generalResponse.ResponseCode == Constants.THARWAT_SUCCESS_CODE ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return PrepareResponse(responseTxt, statusCode);
        }
        

        private HttpResponseMessage PrepareResponse(string responseText, HttpStatusCode responseCode)
        {
           
            var response = new HttpResponseMessage(responseCode)
            {
                Content = new StringContent(responseText, Encoding.UTF8, "application/json")
            };

            return response;
        }
    }
}
