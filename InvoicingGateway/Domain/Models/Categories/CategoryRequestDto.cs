using Domain.Models.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Categories
{
    public class CategoryRequestDto
    {
        [JsonProperty(Required = Required.Default)]
        public int Id { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string CategoryName { get; set; }

        [JsonProperty(Required = Required.Always)]
        public long MerchantId { get; set; }

        [JsonProperty(Required = Required.Default)]
        public string ImageURL { get; set; }

        [JsonProperty(Required = Required.Default)]
        public bool? IsActive { get; set; }

        [JsonProperty(Required = Required.Default)]
        public string Description { get; set; }

        [JsonProperty(Required = Required.Default)]
        public int CategoryType { get; set; }

        public DateTime CreatedAt { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string CustomerId { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string ClientId { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string UserId { get; set; }
        public string IsValid()
        {
            if (string.IsNullOrWhiteSpace(CategoryName))
                return "يرجى ادخال اسم التصنيف ";
            if (MerchantId <= 0)
                return "يرجى ادخال رقم معرف التاجر ";
            /*if (CategoryType <= 0)
                return "يرجى ادخال نوع التصنيف ";*/

            return string.Empty;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
