using Domain.Models;
using Domain.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces.IBusinessLogic.IBase
{
    public interface IBaseInvoiceService<TRequest, TResponse> : IBaseInvoiceGenarlService<TRequest, TResponse>
    {
        public Task<RestAPIGenericResponseDTO<object>> UpdateAsync(TRequest request, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken);
        public Task<RestAPIGenericResponseDTO<object>> DeleteAsync(int id, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken);
        //Task<RestAPIGenericResponseDTO<object>> ChangeStatusAsync(int id, CoreRequest coreRequest, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken);
        Task<RestAPIGenericResponseDTO<object>> ChangeStatusAsync(int id, int status, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken);

    }
    public interface IBaseInvoiceGenarlService<TRequest, TResponse>
    {

        public Task<TResponse> ExecuteAsync(TRequest request, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken);       
        public Task<TResponse> GetById(int id, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken);
        public Task<RestAPIGenericResponseDTO<object>> UpdateStatusAsync( int id, int status, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken);
        public Task<int> GetCountAsync(GeneralFilterDto generalFilter);
    }
}
