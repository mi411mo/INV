using Application.IWebServices;
using Application.Utils;
using IdentityModel.Client;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tharwat.Switch.Utilites.Application.Common.SystemEnviornment;

namespace Infrastructure.WebServicesImpl
{
    public class TokenWebServiceImpl : ITokenWebService
    {
        private readonly IMemoryCache cache;

        public TokenWebServiceImpl(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public async Task<string> GetAccessToken()
        {
            try
            {
                string tokenKey = "invoicing-app";
                string tokenAccess = cache.Get(tokenKey) as string;

                if (tokenAccess == null)
                {
                    // Generate a new token
                    TokenResponse tokenResponse = await GetTokenResponse();
                    var tokenJson = tokenResponse.Json.ToString();
                    var tokenRes = JsonConvert.DeserializeObject<TokenObjectResponse>(tokenJson);
                    await SaveAuthentication(tokenRes, tokenKey);
                    tokenAccess = tokenRes.entity.access_token;
                }
                return tokenAccess;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public async Task<TokenResponse> GetTokenResponse()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            // Pass the handler to httpclient(from you are calling api)
            HttpClient client = new HttpClient(clientHandler);

            string stsURLEnvName = ConfigHelper.Configuration.GetSection("ServiceURLs")["stsURL"];
            var stsURL = SystemEnviornmentLookup.GetEnvVariableValue(stsURLEnvName);

            client.BaseAddress = new Uri(stsURL);
          
            // send custom grant to token endpoint, return response
            var result = await client.RequestTokenAsync(new TokenRequest
            {
                GrantType = ConfigHelper.Configuration.GetSection("PaymentCredentials")["GrantType"],
                ClientId = ConfigHelper.Configuration.GetSection("PaymentCredentials")["ClientId"],
                ClientSecret = ConfigHelper.Configuration.GetSection("PaymentCredentials")["ClientSecret"],

            }); ;

            return result;
        }

        public async Task<TokenObjectResponse> SaveAuthentication(TokenObjectResponse tokenRes, string tokenKey)
        {
            cache.Set(tokenKey, tokenRes.entity.access_token, TimeSpan.FromSeconds(tokenRes.entity.expires_in));
            return tokenRes;
        }
    }
}
