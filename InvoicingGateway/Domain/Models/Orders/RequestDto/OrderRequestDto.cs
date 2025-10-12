using Domain.Models.Invoices.RequestDto;
using Domain.Utils.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Orders.RequestDto
{
    public class OrderRequestDto
    {
        [JsonProperty(Required = Required.Always)]
        public int MerchantId { get; set; }
        [JsonProperty(Required = Required.Always)]
        public decimal TotalAmount { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string CurrencyCode { get; set; }
        [JsonProperty(Required = Required.Default)]
        public int CategoryType { get; set; }
        [JsonProperty(Required = Required.Always)]
        public List<ProductItems> Products { get; set; }
        [JsonProperty(Required = Required.Always)]
        public CustomerInfo CustomerInfo { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string Description { get; set; }
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
                return "يرجى  ادخال رقم معرف التاجر ";

            if (!Products.Any())
                return "يرجى ادخال المنتجات ";
            if (string.IsNullOrWhiteSpace(CustomerInfo.CustomerName))
                return "يرجى ادخال اسم العميل";

            if (string.IsNullOrWhiteSpace(CustomerInfo.Phone))
                return "يرجى ادخال رقم هاتف العميل";

            if (TotalAmount <= 0)
                return "القيمة الاجمالية للطلب غير معرفة";
            if (string.IsNullOrWhiteSpace(CurrencyCode))
                return "عملة الطلب غير معرفة";
           /* if (CategoryType <= 0)
                return "يرجى ادخال نوع التصنيف للطلب";*/

            return string.Empty;
        }

    }

    public class ProductItems
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal AmountDue { get; set; }
        public string CurrencyCode { get; set; }
    }
   
}
