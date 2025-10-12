using Domain.Utils.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.InvoiceCustomParameters
{
    public class InvoiceParametersRequestDto
    {
        [JsonProperty(Required = Required.Default)]
        public int Id { get; set; } 
        [JsonProperty(Required = Required.Always)]
        public int MerchantId { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string ParameterName { get; set; }
        [JsonProperty(Required = Required.Always)]
        public InvoiceCustomParametersTypeEnum ParameterType { get; set; }
        [JsonProperty(Required = Required.Default)]
        public bool? IsActive { get; set; }
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
            if (string.IsNullOrWhiteSpace(ParameterName))
                return "يرجى ادخال اسم المتغير";

            if (string.IsNullOrWhiteSpace(ParameterType.ToString()))
                return "يرجى ادخال نوع المتغير ";
            if (MerchantId < 0)
                return "يرجى ادخال رقم معرف التاجر ";           

            return string.Empty;
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
