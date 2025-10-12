using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IBusinessIndependentService
{
    public interface APIGateway<TResponse, TRequest>
    {
        public Task<TResponse> SendAsync(TRequest request);
    }
}
