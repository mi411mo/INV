using Domain.Models.Base;
using Domain.Utils.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Merchants
{
    public class MerchantRequestDto
    {
        [JsonProperty(Required = Required.Default)]
        public int Id { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string ProfileId { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string ArabicName { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string EnglishName { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string Phone { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string Email { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string Address { get; set; } = null;
        [JsonProperty(Required = Required.Default)]
        public string InvoicePrefix { get; set; } = null;
        [JsonProperty(Required = Required.Always)]
        public IntegrationTypeEnum IntegrationType { get; set; }
        [JsonProperty(Required = Required.Default)]
        public int CategoryType { get; set; }
        [JsonProperty(Required = Required.Default)]
        public AccessChannelEnum AccessChannel { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string Details { get; set; } = null;
        [JsonProperty(Required = Required.Default)]
        public bool? IsActive { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string BusinessCategory { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string LogoImageUrl { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string BusinessDescription { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string WebsiteUrl { get; set; }
        [JsonProperty(Required = Required.Default)]
        public Dictionary<string, string> SocialMedia { get; set; }
        [JsonProperty(Required = Required.Default)]
        public Location StoreLocation { get; set; }
        [JsonProperty(Required = Required.Default)]
        public Dictionary<string, string> OperatingHours { get; set; }
        [JsonProperty(Required = Required.Default)]
        public List<Review> CustomerReviews { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string MarkdownContent { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string CustomerId { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string ClientId { get; set; }
        [JsonProperty(Required = Required.Default)]
        public string UserId { get; set; }

        public string IsValid()
        {
            if (string.IsNullOrWhiteSpace(ProfileId))
                return "يرجى رقم الحساب ";
           /* if (CategoryType <= 0)
                return "يرجى ادخال نوع التصنيف للتاجر";*/
            if (string.IsNullOrWhiteSpace(ArabicName))
                return "يرجى ادخال الاسم ";

            if (string.IsNullOrWhiteSpace(Phone))
                return "يرجى ادخال رقم الهاتف";

            if (string.IsNullOrWhiteSpace(Email))
                return "يرجى ادخال الايميل";
            if (string.IsNullOrWhiteSpace(IntegrationType.ToString()))
                return "يرجى ادخال نوع الارتباط";

            return string.Empty;
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
