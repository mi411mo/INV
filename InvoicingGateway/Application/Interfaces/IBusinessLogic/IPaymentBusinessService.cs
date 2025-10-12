using Application.Interfaces.IBusinessLogic.IBase;
using Domain.Models;
using Domain.Models.Base;
using Domain.Models.Invoices.RequestDto;
using Domain.Models.Payments.RequestDto;
using Domain.Models.Payments.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces.IBusinessLogic
{
    public interface IPaymentBusinessService : IBaseInvoiceService<PaymentRequestDto, RestAPIGenericResponseDTO<PaymentResponseDto>>
    {
        public Task<RestAPIGenericResponseDTO<PaymentResponseDto>> GetAll(GeneralFilterDto generalFilter, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken);
        public Task<RestAPIGenericResponseDTO<PaymentMethodsResponseDto>> GetPaymentMethods(GeneralFilterDto generalFilter, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken);
        Task<RestAPIGenericResponseDTO<ConfirmPaymentResponseDto>> ConfirmPaymentAsync(string payReference, ConfirmPaymentRequestDto request, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken);
        Task<RestAPIGenericResponseDTO<DirectPaymentResponseDto>> DirectPaymentAsync(DirectPaymentRequestDto request, ProfileInfo profileInfo, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken);

        public Task<RestAPIGenericResponseDTO<PaymentResponseDto>> GetPaymentById(int id, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken);
        public Task<RestAPIGenericResponseDTO<PaymentResponseDto>> GetByRef(string paymentRef, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken);
        //Task DirectPaymentAsync(DirectPaymentRequestDto serviceRequest, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsync, HttpRequestMessage request, CancellationToken cancellationToken);

        public Task<RestAPIGenericResponseDTO<CheckProviderPaymentStatusResponseDto>> CheckProviderPaymentStatus(string paymentRef, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncDelegate, HttpRequestMessage httpRequest, CancellationToken cancellationToken);
    }
}
