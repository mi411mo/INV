using Application.Interfaces.IBusinessLogic.IBase;
using Domain.Models;
using Domain.Models.Base;
using Domain.Models.Invoices.RequestDto;
using Domain.Models.Invoices.ResponseDto;
using Domain.Models.Orders.RequestDto;
using Domain.Models.Orders.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces.IBusinessLogic
{
    public interface IInvoiceBusinessService : IBaseInvoiceService<InvoiceRequestDto, RestAPIGenericResponseDTO<InvoiceResponseDto>>
    {
        public Task<RestAPIGenericResponseDTO<InvoiceResponseDto>> GetAll(GeneralFilterDto generalFilter, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken);
        Task<RestAPIGenericResponseDTO<InvoiceConfirmResponseDto>> ConfirmInvoiceAsync(string invNo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken);
        public Task<RestAPIGenericResponseDTO<InvoiceResponseDto>> GetByRef(string invoiceNumber, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken);
    }
}
