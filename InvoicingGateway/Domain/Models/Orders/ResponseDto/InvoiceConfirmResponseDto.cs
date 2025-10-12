using Domain.Entities;
using Domain.Models.Invoices.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Orders.ResponseDto
{
    public class InvoiceConfirmResponseDto
    {
        public string InvoiceReference { get; set; }
        public string PaymentReference { get; set; }
        public decimal TotalAmount { get; set; }
        public string CurrencyCode { get; set; }
        public string TransactionURL { get; set; }

        public InvoiceConfirmResponseDto ToResponse(InvoiceModel invoice, string transUrl, string paymentReference)
        {
            return new InvoiceConfirmResponseDto()
            {
                InvoiceReference = invoice.InvoiceNumber,
                PaymentReference = paymentReference,
                TotalAmount = invoice.TotalAmountDue,
                CurrencyCode = invoice.CurrencyCode,
                TransactionURL = transUrl
            };
        }
    }
}
