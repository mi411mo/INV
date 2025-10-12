using Application.Interfaces.IBusinessLogic.IBase;
using Domain.Models;
using Domain.Models.Base;
using Domain.Models.Orders.ResponseDto;
using Domain.Models.Services;
using Domain.Models.Services.RequestDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces.IBusinessLogic
{
    public interface IProductBusinessService :  IBaseInvoiceService<ProductRequestDto, RestAPIGenericResponseDTO<ProductResponseDto>>
    {
        public Task<RestAPIGenericResponseDTO<ProductResponseDto>> GetAll(GeneralFilterDto generalFilter, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken);
    }
}
