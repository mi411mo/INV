using Domain.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Services.RequestDto
{
    public class ProductRequestDto : GeneralDto
    {
        [JsonProperty(Required = Required.Default)]
        public int Id { get; set; }
        [JsonProperty(Required = Required.Default)]
        public decimal Quantity { get; set; }
        [JsonProperty(Required = Required.Always)]
        public decimal TotalAmount { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string CurrencyCode { get; set; }
        [JsonProperty(Required = Required.Default)]
        public int CategoryType { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string ImageURL { get; set; }
        [JsonProperty(Required = Required.Always)]
        public int CategoryId { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string SubCategoryId { get; set; }
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
            if (MerchantId <= 0)
                return "يرجى ادخال رقم التاجر للمنتج";
           /* if (CategoryType <= 0)
                return "يرجى ادخال نوع التصنيف للمنتج";*/
            if (CategoryId <= 0)
                return "يرجى ادخال رقم الفئة للمنتج";
            // validate amount
            var amtCheck = ValidationUtility.ValidAmount(TotalAmount);
            if (!string.IsNullOrEmpty(amtCheck))
                return amtCheck;
            var quantityCheck = ValidationUtility.ValidQuantity(Quantity);
            if (!string.IsNullOrEmpty(quantityCheck))
                return quantityCheck;
            var priceCheck = ValidationUtility.ValidPrice(UnitPrice);
            if (!string.IsNullOrEmpty(priceCheck))
                return priceCheck;
            if (string.IsNullOrWhiteSpace(CurrencyCode))
                return "يرجى ادخال العملة";

            return string.Empty;
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

    }
    
}
