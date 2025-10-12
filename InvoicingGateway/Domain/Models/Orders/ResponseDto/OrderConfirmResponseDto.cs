using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Orders.ResponseDto
{
    public class OrderConfirmResponseDto
    {
        public string OrderReference { get; set; }
        public decimal TotalAmount { get; set; }
        public string CurrencyCode { get; set; }
        public string TransactionURL { get; set; }

        public OrderConfirmResponseDto ToResponse(InvoiceModel invoice, string transUrl)
        {
            return new OrderConfirmResponseDto()
            {
                OrderReference = invoice.InvoiceNumber,
                TotalAmount = invoice.TotalAmountDue,
                CurrencyCode = invoice.CurrencyCode,
                TransactionURL = transUrl
            };
        }

        public OrderConfirmResponseDto ToResponse(OrderModel order, string transUrl)
        {
            return new OrderConfirmResponseDto()
            {
                OrderReference = order.OrderReference,
                TotalAmount = order.TotalAmount,
                CurrencyCode = order.CurrencyCode,
                TransactionURL = transUrl
            };
        }

    }
}
