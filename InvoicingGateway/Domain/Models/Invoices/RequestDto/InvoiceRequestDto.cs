using Domain.Models.InvoiceCustomValues;
using Domain.Models.Orders.RequestDto;
using Domain.Utils;
using Domain.Utils.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Invoices.RequestDto
{
    public class InvoiceRequestDto
    {
        [JsonProperty(Required = Required.Default)]
        public int OrderId { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string OrderReference { get; set; }
        [JsonProperty(Required = Required.Default)]
        public int MerchantId { get; set; }
        [JsonProperty(Required = Required.Always)]
        public decimal TotalAmount { get; set; }
       /* [JsonProperty(Required = Required.Default)]
        public decimal AmountPaid { get; set; }
        [JsonProperty(Required = Required.Default)]
        public decimal AmountRemaining { get; set; }*/
        [JsonProperty(Required = Required.Default)]
        public decimal AmountShipping { get; set; }
        [JsonProperty(Required = Required.Default)]
        public decimal Discount { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string CurrencyCode { get; set; }
        [JsonProperty(Required = Required.Always)]
        public CustomerInfo Customer { get; set; }
        [JsonProperty(Required = Required.Default)]
        public int CategoryType { get; set; }
        [JsonProperty(Required = Required.Always)]
        public List<ProductItems> Products { get; set; }
        [JsonProperty(Required = Required.Default)]
        public List<InvoiceCustomValueRequest>? CustomValues { get; set; }
        /*[JsonProperty(Required = Required.Default)]
        public InvoiceStatusEnums Status { get; set; }*/
        /*[JsonProperty(Required = Required.Default)]
        public string[] AcceptedCurrencies { get; set; }*/
        [JsonProperty(Required = Required.Default)]
        public string[] PaymentMethods { get; set; }
        [JsonProperty(Required = Required.Default)]
        public Notification Notification { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string Description { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string CustomerId { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string ClientId { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string UserId { get; set; }
        public string IsValid()
        {
            /*if (string.IsNullOrWhiteSpace(OrderId.ToString()))
                return "يرجى ادخال رقم الطلب";*/
            /*if (string.IsNullOrWhiteSpace(CustomerInfo.ToString()))
                return "يرجى ادخال معلومات العميل";*/
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

    public class CustomerInfo
    {
        [JsonProperty(Required = Required.Default)]
        public int CustomerId { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string? CustomerName { get; set; } = string.Empty;
        [JsonProperty(Required = Required.Default)]
        public string? Phone { get; set; } = string.Empty;
        [JsonProperty(Required = Required.Default)]
        public string? Email { get; set; } = string.Empty;
        [JsonProperty(Required = Required.Default)]
        public string? Country { get; set; } = string.Empty;
        [JsonProperty(Required = Required.Default)]
        public string? Governorate { get; set; } = string.Empty;
        [JsonProperty(Required = Required.Default)]
        public string? Address { get; set; } = string.Empty;          
        [JsonProperty(Required = Required.Default)]
        public string? AddressType { get; set; } = string.Empty;
    }

    public class Notification
    {
        [JsonProperty(Required = Required.Default)]
        public string[] Channels { get; set; }
        [JsonProperty(Required = Required.Default)]
        public bool Dispatch { get; set; }
    }

    
}
