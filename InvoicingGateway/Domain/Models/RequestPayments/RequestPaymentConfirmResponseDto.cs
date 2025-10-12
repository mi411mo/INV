using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.RequestPayments
{
    public class RequestPaymentConfirmResponseDto
    {
        public string RequestPaymentReference { get; set; }
        public decimal TotalAmount { get; set; }
        public string CurrencyCode { get; set; }
        public string TransactionURL { get; set; }

        public RequestPaymentConfirmResponseDto ToResponse(RequestPayment reqPay, string transUrl)
        {
            return new RequestPaymentConfirmResponseDto()
            {
                RequestPaymentReference = reqPay.RequestPaymentReference,
                TotalAmount = reqPay.TotalAmount,
                CurrencyCode = reqPay.CurrencyCode,
                TransactionURL = transUrl
            };
        }
    }
}
