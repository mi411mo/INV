using Domain.Utils.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.GrpcDto.RequestDto
{
    public class ServiceRatingRequestDto
    {
		[JsonProperty(Required = Required.Default)]
		public string CustomerProfileId { get; set; }
		[JsonProperty(Required = Required.Default)]
		public string ProviderProfileId { get; set; }
		[JsonProperty(Required = Required.Default)]
		public string CurrencyCode { get; set; }
		public string Amount { get; set; }
		public string CompanyCode { get; set; }
		[JsonProperty(Required = Required.Default)]
		public string ServiceCode { get; set; }
		[JsonProperty(Required = Required.Default)]
		public string ServiceType { get; set; } = ((int)ServiceTypeEnum.Purchases).ToString();
	}
}
