using Domain.Utils.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Services.RequestDto
{
    public class GeneralDto
    {
        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }
        [JsonProperty(Required = Required.Default)]
        public ProductsStatusEnum Status { get; set; }
        [JsonProperty(Required = Required.Default)]
        public decimal UnitPrice { get; set; }
        [JsonProperty(Required = Required.Default)]
        public decimal Discount { get; set; }
        [JsonProperty(Required = Required.Always)]
        public int MerchantId { get; set; }
    }
}
