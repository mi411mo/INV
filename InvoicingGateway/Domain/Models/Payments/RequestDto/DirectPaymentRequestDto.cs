using Domain.Models.Invoices.RequestDto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Payments.RequestDto
{
    public class DirectPaymentRequestDto
    {
        [JsonProperty(Required = Required.Default)]
        public string RequestId { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string ReferenceNumber { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string ServiceCode { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string InvoiceReference { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string TransactionReference { get; set; }

        [JsonProperty(Required = Required.Default)]
        public string? PaymentMethod { get; set; }
        [JsonProperty(Required = Required.Always)]
        public decimal Amount { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string CurrencyCode { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string Details { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string CustomerId { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string ClientId { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string UserId { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public string IsValid()
        {
            if (Amount <= 0)
                return " المبلغ غير صحيح";
            if (string.IsNullOrWhiteSpace(CurrencyCode))
                return "عملة الدفع غير معرفة";

            return string.Empty;
        }
    }
}
