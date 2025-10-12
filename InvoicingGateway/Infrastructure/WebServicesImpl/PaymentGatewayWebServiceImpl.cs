using Application.DTOs;
using Application.IWebServices;
using Application.Utils;
using Domain.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Tharwat.Switch.Utilites.Application.Common.SystemEnviornment;

namespace Infrastructure.WebServicesImpl
{
    public class PaymentGatewayWebServiceImpl : IPaymentGatewayWebService
    {
        private readonly ITokenWebService tokenWebService;

        public PaymentGatewayWebServiceImpl(ITokenWebService tokenWebService)
        {
            this.tokenWebService = tokenWebService;
        }

        public async Task<CheckPortalPaymentStatusResponse> CheckPortalPaymentStatus(string requestId)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                var tokenAccess = await tokenWebService.GetAccessToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAccess);

                try
                {
                    var baseCheckPayEnvName = ConfigHelper.Configuration.GetSection("ServiceURLs")["CheckLinkPaymentStatusUrl"];
                    var baseCheckPayUrl = SystemEnviornmentLookup.GetEnvVariableValue(baseCheckPayEnvName.Trim());


                    var checkPayStatusUrl = baseCheckPayUrl + requestId.Trim();
                    var response = await client.GetAsync(checkPayStatusUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        var payMethodRes = JsonConvert.DeserializeObject<CheckPortalPaymentStatusResponse>(responseString);
                        return payMethodRes;
                    }
                    else
                    {
                        //TODO: Decide what to return 
                        return new CheckPortalPaymentStatusResponse()
                        {
                            Success = false,
                            ResponseCode = Constants.THARWAT_SUCCESS_CODE,
                            Message = Constants.SUCCESS_Message
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new CheckPortalPaymentStatusResponse()
                    {
                        Success = false,
                        ResponseCode = Constants.SERVER_ERROR_CODE,
                        Message = Constants.SUCCESS_Message
                    };
                }

            }
        }

        public async Task<PaymentMethodsResponse> GetPaymentMethods()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                var tokenAccess = await tokenWebService.GetAccessToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAccess);

                /*var json = JsonConvert.SerializeObject(string.Empty);
                var content = new StringContent(json, Encoding.UTF8, "application/json");*/
                try
                {
                    var paymentMethodEnvName = ConfigHelper.Configuration.GetSection("ServiceURLs")["PaymentMethodsUrl"];
                    var paymentMethodUrl = SystemEnviornmentLookup.GetEnvVariableValue(paymentMethodEnvName);

                    var response = await client.GetAsync(paymentMethodUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        var payMethodRes = JsonConvert.DeserializeObject<PaymentMethodsResponse>(responseString);
                        return payMethodRes;
                    }
                    else
                    {
                        //TODO: Decide what to return 
                        return new PaymentMethodsResponse()
                        {
                            Success = false,
                            ResponseCode = Constants.THARWAT_SUCCESS_CODE,
                            Message = Constants.SUCCESS_Message
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new PaymentMethodsResponse()
                    {
                        Success = false,
                        ResponseCode = Constants.THARWAT_SUCCESS_CODE,
                        Message = Constants.SUCCESS_Message
                    };
                }

            }
        }

        public async Task<RedirectLinkPaymentsResponse> RedirectLinkPayments(RedirectLinkPaymentsRequest  redirectRequest)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                var tokenAccess = await tokenWebService.GetAccessToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAccess);

                var json = JsonConvert.SerializeObject(redirectRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    var checkoutUrlEnvName = ConfigHelper.Configuration.GetSection("ServiceURLs")["CheckoutUrl"];
                    var checkoutUrl = SystemEnviornmentLookup.GetEnvVariableValue(checkoutUrlEnvName);

                    var response = await client.PostAsJsonAsync(checkoutUrl, redirectRequest, new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    });

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        var redirectResponse = JsonConvert.DeserializeObject<RedirectLinkPaymentsResponse>(responseString);
                        return redirectResponse;
                    }
                    else
                    {
                        //TODO: Decide what to return 
                        return new RedirectLinkPaymentsResponse()
                        {
                            Success = true,
                            ResponseCode = Constants.THARWAT_SUCCESS_CODE,
                            Message = Constants.SUCCESS_Message,
                            Entity = new Application.DTOs.Base.Entity
                            {
                                TransactionUrl= ""
                            }
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new RedirectLinkPaymentsResponse()
                    {
                        Success = true,
                        ResponseCode = Constants.THARWAT_SUCCESS_CODE,
                        Message = Constants.SUCCESS_Message,
                        Entity = new Application.DTOs.Base.Entity
                        {
                            TransactionUrl = ""
                        }
                    };
                }

            }
        }
    }
}
