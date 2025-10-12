using Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils
{
    public class HTTPHelper<T>
    {
        private static HTTPHelper<T> httpHelper;

        public static HTTPHelper<T> GetIsntance()
        {
            if (httpHelper == null)
            {
                httpHelper = new HTTPHelper<T>();
            }

            return httpHelper;
        }

        public async Task<T> GetBodyModel(HttpRequestMessage request)
        {
            var res = await request.Content.ReadAsStringAsync();
            // deserialize request body (string) to  model
            var requestModel = JSONHelper<T>.GetTypedModel(res);

            return requestModel;
        }

        public async Task<T> GetBodyModel(HttpResponseMessage response)
        {
            var res = await response.Content.ReadAsStringAsync();
            // deserialize response body (string) to  model
            var responseModel = JSONHelper<T>.GetTypedModel(res);

            return responseModel;
        }

        public Uri GetURI(Uri uri, string host, int port, string path)
        {
            // assign IP, port  & path to this request
            var newUri = new UriBuilder(uri);
            newUri.Host = host;
            newUri.Port = port;
            newUri.Path = path;

            return newUri.Uri;

        }
    }
}
