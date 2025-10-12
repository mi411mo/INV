using Domain.Models.InvoiceCustomValues;
using Domain.Models.Invoices.RequestDto;
using Domain.Models.Orders.RequestDto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Invoices.ResponseDto
{
    public class InvoiceResponseDto
    {
        public long? Id { get; set; } = null;
        public int MerchantId { get; set; }
        public string InvoiceNumber { get; set; }
        public string OrderReference { get; set; }
        public long PaymentToken { get; set; }
        public int OrderId { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public CustomerInfo Customer { get; set; } = null;
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public List<ProductItems> Products { get; set; } = null;
        public List<InvoiceCustomValueResponse> CustomParameters { get; set; } = null;
        public decimal TotalAmountDue { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal AmountRemaining { get; set; }
        public decimal AmountShipping { get; set; }
        public decimal Discount { get; set; }
        public string CurrencyCode { get; set; }
        public int? CategoryType { get; set; }
        public int Status { get; set; }
        //public string AcceptedCurrencies { get; set; }
        public string[] PaymentMethods { get; set; }
        public Notification Notification { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } = null;
    }
}
