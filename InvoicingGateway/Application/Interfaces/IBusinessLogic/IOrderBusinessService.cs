using Application.Interfaces.IBusinessLogic.IBase;
using Domain.Entities;
using Domain.Models;
using Domain.Models.Base;
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
    public interface IOrderBusinessService : IBaseInvoiceService<OrderRequestDto, RestAPIGenericResponseDTO<OrderResponseDto>>
    {
        public Task<RestAPIGenericResponseDTO<GetOrderResponseDto>> GetAll(GeneralFilterDto generalFilter, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken);
        Task<RestAPIGenericResponseDTO<OrderConfirmResponseDto>> ConfirmOrderAsync(string orderRef, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken);
        public Task<RestAPIGenericResponseDTO<GetOrderResponseDto>> GetOrderById(int id, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken);
        public Task<RestAPIGenericResponseDTO<GetOrderResponseDto>> GetByRef(string orderRef, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken);
    }
}
