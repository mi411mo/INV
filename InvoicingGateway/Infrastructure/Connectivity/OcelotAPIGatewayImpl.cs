using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Application.IBusinessIndependentService;
using Microsoft.Extensions.Logging;

namespace Connectivity.OcelotAPIGatewayImpl
{
    public class OcelotAPIGatewayImpl : IOcelotAPIGateway
    {
        private readonly ILogger<OcelotAPIGatewayImpl> logger;

        public OcelotAPIGatewayImpl(ILogger<OcelotAPIGatewayImpl> logger)
        {
            this.logger = logger;
        }
        public async Task<HttpResponseMessage> SendAsync(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncFunctionDelegate, HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await sendAsyncFunctionDelegate(request, cancellationToken);

                return response;
            }
            catch (WebException webExp)
            {
                logger.LogError("WebException has been thrown while communicating with the Backend Service, reason:{}", webExp.Message);

                throw webExp;
            }
            catch (Exception generalExp)
            {
                logger.LogError("GeneralException has been thrown while communicating with the Backend Service, reason:{}", generalExp.Message);

                throw generalExp;
            }
        }

        public Task<Task<HttpResponseMessage>> SendAsync(HttpRequestMessage request)
        {
            throw new NotImplementedException();
        }
    }
}
