using Domain.Entities;
using Domain.Models.Invoices.RequestDto;
using Domain.Utils.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Payments.ResponseDto
{
    public class PaymentResponseDto
    {
        public long? Id { get; set; }
        public string TransactionReference { get; set; }
        public string TargetReference { get; set; }
        public string InvoiceReference { get; set; }
        public string OrderReference { get; set; }
        public string RequestPaymentReference { get; set; }
        public string PaymentReference { get; set; }
        public int MerchantId { get; set; }
        public CustomerInfo CustomerInfo { get; set; }
        public string PaymentMethod { get; set; }
        public PaymentStatusEnum PaymentStatus { get; set; }
        public PaymentTypeEnum PaymentType { get; set; }
        public decimal AmountDue { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? PayDate { get; set; } = null;
        public string Details { get; set; }

        public PaymentResponseDto ToResponse(Payment entity)
        {
            return new PaymentResponseDto()
            {
                Id = entity.Id,
                TransactionReference = entity.TransactionReference,
                TargetReference = entity.TargetReference,
                InvoiceReference = entity.InvoiceReference,
                OrderReference = entity.OrderReference,
                MerchantId = entity.MerchantId,
                CustomerInfo = string.IsNullOrEmpty(entity.CustomerInfo) ? null : JsonConvert.DeserializeObject<CustomerInfo>(entity.CustomerInfo),
                CurrencyCode = entity.CurrencyCode,
                AmountDue = entity.AmountDue,
                IssueDate = entity.IssueDate,
                PayDate = entity.PayDate,
                Details = entity.Details
            };

        }
    }
}
