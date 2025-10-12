using Application.DTOs;
using Application.Utils;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebAPI.Utils
{
    public class AuthorizationHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthorizationHelper(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<HttpResponseMessage> ValidateUserPermissionsAsync(string permissions)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var user = httpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }

            var userId = user.FindFirst("sub")?.Value;
            var customrId = user.FindFirst("CustomerId")?.Value;
            var accessToken = GetToken();

            if (userId != null)
            {
                // Split permissions on "||" to check each one
                var permissionList = permissions.Split(new[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                bool hasPermission = false;

                foreach (var permission in permissionList)
                {
                    hasPermission = await HasPermission(permission.Trim(), accessToken, userId);
                    if (hasPermission)
                    {
                        break; 
                    }
                }

                if (!hasPermission)
                {
                    httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
                    {
                        Content = new StringContent("You do not have permission to access this resource.")
                    };
                }
            }

            return null;
        }
        public async Task<HttpResponseMessage> ValidateUserPermissionsAsync(string permissions, string scope)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var user = httpContext.User;

            if (user?.Identity == null || !user.Identity.IsAuthenticated)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }

            var userId = user.FindFirst("sub")?.Value;
            var customerId = user.FindFirst("CustomerId")?.Value; 
            var accessToken = GetToken();

            if (!string.IsNullOrEmpty(userId))
            {
                var permissionList = permissions.Split(new[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                bool hasPermission = false;

                foreach (var permission in permissionList)
                {
                    hasPermission = await HasPermission(permission.Trim(), accessToken, userId);
                    if (hasPermission)
                    {
                        break;
                    }
                }

                if (!hasPermission)
                {
                    return new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
                    {
                        Content = new StringContent("You do not have permission to access this resource.")
                    };
                }
            }
            else
            {
                if (user.Identity is ClaimsIdentity claimsIdentity)
                {
                    var scopes = claimsIdentity.FindAll("scope")
                                               .Select(c => c.Value)
                                               .ToList();

                    if (scopes.Count == 1 && scopes[0].Contains(' '))
                    {
                        scopes = scopes[0]
                                 .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                 .ToList();
                    }
                    customerId = claimsIdentity.FindFirst("client_CustomerRelatedId").Value;
                    bool scopeExists = scopes.Any(s => s.Equals(scope, StringComparison.OrdinalIgnoreCase));

                    if (!scopeExists)
                    {
                        return new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
                        {
                            Content = new StringContent("You do not have the required scope to access this resource.")
                        };
                    }
                }
                else
                {
                    return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                }
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }

        public UserAuditData GetUserAuditData()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var user = httpContext?.User;

            if (user == null)
            {
                return new UserAuditData();
            }

            string GetClaimValue(string claimType)
            {
                return user.FindFirst(claimType)?.Value;
            }

            var userId = GetClaimValue("sub");
            var clientId = GetClaimValue("client_id");
            string customerId = null;

            if (userId == null)
            {
                if (user.Identity is ClaimsIdentity claimsIdentity)
                {
                    customerId = claimsIdentity.FindFirst("client_CustomerRelatedId")?.Value;
                }
            }
            else
            {
                customerId = GetClaimValue("CustomerId");
            }

            return new UserAuditData
            {
                UserId = userId,
                ClientId = clientId,
                CustomerId = customerId
            };
        }
        private string GetToken()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var authHeader = httpContext.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                return authHeader.Substring("Bearer ".Length).Trim();
            }
            return null;
        }

        private async Task<bool> HasPermission(string permission, string accessToken, string userId)
        {
            try
            {
                var result = await GetUserPermissionsAsync(accessToken, userId, permission);
                return result.Contains("true", StringComparison.OrdinalIgnoreCase); 
            }
            catch (Exception ex)
            {
                // Log the exception (logging logic can be added)
                return false;
            }
        }

        private async Task<string> GetUserPermissionsAsync(string accessToken, string userId, string permission)
        {
            try
            {
                using var httpClient = _httpClientFactory.CreateClient("IdentityService");
                var url = GeneralVar.GetUserPermissionByUserURL() + userId + "&permission=" + permission;

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();
                var responseJson = await response.Content.ReadAsStringAsync();

                return responseJson;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}