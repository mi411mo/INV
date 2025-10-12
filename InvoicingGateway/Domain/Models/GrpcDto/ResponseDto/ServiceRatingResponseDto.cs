using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.GrpcDto.ResponseDto
{
    public class ServiceRatingResponseDto
    {
		public ServiceRatingResponseDto()
		{

		}
		public bool Success { set; get; }
		public string ResponseCode { set; get; }
		public string Message { set; get; }
		public string TotalAmount { set; get; }
		public string Currency { set; get; }
		public string Amount { set; get; }
		public string CommissionTotal { set; get; } = "0";
		public string CommissionCurrency { set; get; }
		public string FeesTotal { set; get; } = "0";
		public string FeesCurrency { set; get; }
		public string ExtraTotal { set; get; } = "0";
		public string ExtraCurrency { set; get; }
		public string TotalRate { set; get; } = "0";
		public List<DetailsReply> FeesDetail { get; set; }
		public List<DetailsReply> CommissionDetails { get; set; }
		public List<DetailsReply> ExtraDetails { get; set; }
	}

	public class DetailsReply
	{
		public string Id { get; set; }
		public string Amount { get; set; }
		public string Name { get; set; }
		public string Currency { get; set; }
		public string CalculationType { get; set; } = null;
		public string ServideRatingCode { get; set; } = null;
		public string RatingPoliceName { get; set; } = null;

	}
}

