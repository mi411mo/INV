using Domain.Models.Invoices.RequestDto;
using Domain.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.RequestPayments
{
    public class RequestPaymentRequestDto
    {
        [JsonProperty(Required = Required.Always)]
        public int MerchantId { get; set; }
        [JsonProperty(Required = Required.Default)]
        public CustomerInfo Customer { get; set; }
        [JsonProperty(Required = Required.Always)]
        public decimal TotalAmount { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string CurrencyCode { get; set; }
        [JsonProperty(Required = Required.Default)]
        public int? CategoryType { get; set; }
        [JsonProperty(Required = Required.Default)]
        public int Status { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string PaymentMethod { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string Description { get; set; }
        [JsonProperty(Required = Required.Default)]
        public DateTime CreatedAt { get; set; }
        [JsonProperty(Required = Required.Default)]
        public DateTime? UpdatedAt { get; set; } = null;
        [JsonProperty(Required = Required.Default)]
        public string CustomerId { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string ClientId { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string UserId { get; set; }

        public string IsValid()
        {
            // validate amount
            var amtCheck = ValidationUtility.ValidAmount(TotalAmount);
            if (!string.IsNullOrEmpty(amtCheck))
                return amtCheck;
            if (string.IsNullOrWhiteSpace(CurrencyCode))
                return "يرجى ادخال رمز العملة";
            /* if (CategoryType <= 0)
                 return "يرجى ادخال نوع التصنيف للفاتورة";*/

            return string.Empty;
        }
    }
}
