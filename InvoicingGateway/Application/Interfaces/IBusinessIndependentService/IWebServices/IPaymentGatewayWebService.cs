using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IWebServices
{
    public interface IPaymentGatewayWebService
    {
        Task<RedirectLinkPaymentsResponse> RedirectLinkPayments(RedirectLinkPaymentsRequest redirectRequest);
        Task<PaymentMethodsResponse> GetPaymentMethods();
        Task<CheckPortalPaymentStatusResponse> CheckPortalPaymentStatus(string requestId);
    }
}
