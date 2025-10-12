using Domain.Models.Invoices.RequestDto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Payments.RequestDto
{
    public class PaymentRequestDto
    {
        [JsonProperty(Required = Required.Default)]
        public string TransactionReference { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string TargetReference { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string InvoiceReference { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string OrderReference { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string PaymentReference { get; set; }
        [JsonProperty(Required = Required.Default)]
        public int MerchantId { get; set; }
        [JsonProperty(Required = Required.Default)]
        public CustomerInfo CustomerInfo { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string? PaymentMethod { get; set; }
        [JsonProperty(Required = Required.Default)]
        public decimal AmountDue { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string CurrencyCode { get; set; }
        //public DateTime IssueDate { get; set; }
        //public DateTime PayDateDate { get; set; }
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
            if (string.IsNullOrWhiteSpace(MerchantId.ToString()))
                return "يرجى ادخال رقم معرف التاجر ";

            if (AmountDue <= 0)
                return "القيمة الاجمالية للدفع غير معرفة";
            if (string.IsNullOrWhiteSpace(CurrencyCode))
                return "عملة الدفع غير معرفة";

            return string.Empty;
        }

    }

}
