using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Customers
{
    public class CustomerRequestDto
    {
        [JsonProperty(Required = Required.Default)]
        public int Id { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string ProfileId { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string Phone { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string Email { get; set; }
        [JsonProperty(Required = Required.Always)]
        public int MerchantId { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string? Address { get; set; } = null;
        [JsonProperty(Required = Required.Default)]
        public string? InvoicePrefix { get; set; } = null;
        [JsonProperty(Required = Required.Default)]
        public int CategoryType { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string? Details { get; set; } = null;
        [JsonProperty(Required = Required.Default)]
        public bool? IsActive { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string CustomerId { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string ClientId { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string UserId { get; set; }
        public string IsValid()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return "يرجى ادخال الاسم ";
            if (MerchantId < 0)
                return "يرجى ادخال رقم معرف التاجر ";

            if (string.IsNullOrWhiteSpace(Phone))
                return "يرجى ادخال رقم الهاتف";

           /* if (string.IsNullOrWhiteSpace(Email))
                return "يرجى ادخال الايميل";*/
            /*if (CategoryType <= 0)
                return "يرجى ادخال نوع التصنيف للعميل";*/

            return string.Empty;
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
