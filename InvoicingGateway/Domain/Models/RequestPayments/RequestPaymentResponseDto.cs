using Domain.Models.Invoices.RequestDto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.RequestPayments
{
    public class RequestPaymentResponseDto
    {
        public long? Id { get; set; } = null;
        public int MerchantId { get; set; }
        public string RequestPaymentReference { get; set; }
        //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public CustomerInfo Customer { get; set; } = null;
        public decimal TotalAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public string CurrencyCode { get; set; }
        public int? CategoryType { get; set; }
        public int Status { get; set; }
        public string PaymentMethod { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } = null;
    }
}
