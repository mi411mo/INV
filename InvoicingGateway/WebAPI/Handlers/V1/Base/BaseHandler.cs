
using Application.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Utils;

namespace Tharwat.Invoices.Gateway.Handlers.V1.Base
{
    public class BaseHandler<T> : DelegatingHandler
    {
      
        protected readonly ILogger<T> logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor accessor;
        protected readonly IConfiguration config;
        private readonly AuthorizationHelper _authorizationHelper;

        public BaseHandler(ILogger<T> logger, IHttpClientFactory
            _httpClientFactory, IHttpContextAccessor accessor, IConfiguration config, AuthorizationHelper _authorizationHelper)
        {
            //System.Diagnostics.Debug.WriteLine($"{typeof(T).Name} : Handlers()");
            this.logger = logger;
        
            this._httpClientFactory = _httpClientFactory;
            this.accessor = accessor;
            this.config = config;
            this._authorizationHelper = _authorizationHelper;
        }
        protected async Task<HttpResponseMessage> CheckPostHttpMethod(HttpRequestMessage httpRequest)
        {
            if (httpRequest.Method != HttpMethod.Post)
                return await Task.FromResult(new HttpResponseMessage(HttpStatusCode.MethodNotAllowed)
                {
                    Content = new StringContent("", Encoding.UTF8, "application/json")
                });
            return null;
            // return await Task.FromResult(httpRequest.Method == HttpMethod.Post);
        }

      
        public string GetToken()
        {

            var httpContext = accessor.HttpContext;

            // Get the Authorization header
            var authHeader = httpContext.Request.Headers["Authorization"].ToString();

            string token = null;

            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                // Extract the token
                token = authHeader.Substring("Bearer ".Length).Trim();
            }
            return token;
        }

        protected async Task<bool> GetPermission(string permission, string userId, string token)
        {
            try
            {
                var GetUserPermissions = GetUserPermissionsAsync(token, userId, permission);
                if (GetUserPermissions.Result.Contains("true"))
                {
                    return true;
                }
                else if (GetUserPermissions.Status.ToString() == "WaitingForActivation")
                {
                    return false;
                }

                else
                    return false;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        protected async Task<bool> GetPermissions(string policy, string accessToken, string userId)
        {

            var permission = GetPermission(policy, userId, accessToken);
            return permission.Result;

        }
        private async Task<string> GetUserPermissionsAsync(string accessToken, string userId, string permision)
        {
            try
            {


                using var httpClient = _httpClientFactory.CreateClient("IdentityService");

                var url = GeneralVar.GetUserPermissionByUserURL() + userId + "&permission=" + permision;

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var responsejson = await response.Content.ReadAsStringAsync();

                return responsejson;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    

      
    }
}
