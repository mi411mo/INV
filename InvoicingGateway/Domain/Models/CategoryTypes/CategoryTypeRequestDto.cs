using Domain.Utils.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.CategoryTypes
{
    public class CategoryTypeRequestDto
    {
        [JsonProperty(Required = Required.Default)]
        public int Id { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string EnName { get; set; }
        [JsonProperty(Required = Required.Default)]
        public CategoryTypeEnum Type { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string Code { get; set; }
        [JsonProperty(Required = Required.Default)]
        public bool? IsActive { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string Description { get; set; }
        [JsonProperty(Required = Required.Default)]
        public DateTime CreatedAt { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string CustomerId { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string ClientId { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string UserId { get; set; }
        public string IsValid()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return "يرجى ادخال اسم التصنيف ";
            if (string.IsNullOrWhiteSpace(EnName))
                return "يرجى ادخال الاسم الانجليزي للتصنيف";
            if (string.IsNullOrWhiteSpace(Type.ToString()))
                return "يرجى ادخال نوع التصنيف ";

            return string.Empty;
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
