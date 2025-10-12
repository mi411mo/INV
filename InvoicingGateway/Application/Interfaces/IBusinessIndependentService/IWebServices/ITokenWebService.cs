using Application.Utils;
using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IWebServices
{
    public interface ITokenWebService
    {
        Task<string> GetAccessToken();
        Task<TokenResponse> GetTokenResponse();
        Task<TokenObjectResponse> SaveAuthentication(TokenObjectResponse tokenRes, string tokenKey);
    }
}
