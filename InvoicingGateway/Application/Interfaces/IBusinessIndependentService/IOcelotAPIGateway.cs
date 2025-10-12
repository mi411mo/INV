using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.IBusinessIndependentService
{
    public interface IOcelotAPIGateway : APIGateway<Task<HttpResponseMessage>, HttpRequestMessage>
    {
        public Task<HttpResponseMessage> SendAsync(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncFunctionDelegate, HttpRequestMessage request, CancellationToken cancellationToken);
    }  
    
}
