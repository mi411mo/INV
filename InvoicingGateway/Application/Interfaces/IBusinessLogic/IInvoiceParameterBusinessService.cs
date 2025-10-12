using Application.Interfaces.IBusinessLogic.IBase;
using Domain.Models;
using Domain.Models.Base;
using Domain.Models.CategoryTypes;
using Domain.Models.InvoiceCustomParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces.IBusinessLogic
{
    public interface IInvoiceParameterBusinessService : IBaseInvoiceService<InvoiceParametersRequestDto, RestAPIGenericResponseDTO<InvoiceParametersResponseDto>>
    {
        public Task<RestAPIGenericResponseDTO<InvoiceParametersResponseDto>> GetAll(GeneralFilterDto generalFilter, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken);
    }
}
