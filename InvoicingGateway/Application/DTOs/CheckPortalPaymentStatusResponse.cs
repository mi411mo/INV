using Domain.Models.Payments.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CheckPortalPaymentStatusResponse
    {
        public bool Success { get; set; }
        public string ResponseCode { get; set; }
        public string Message { get; set; }
        public ResposeEntity  entity { get; set; }

        public CheckProviderPaymentStatusResponseDto ToResponse (CheckPortalPaymentStatusResponse response)
        {
            return new CheckProviderPaymentStatusResponseDto()
            {
                RequestId = response.entity.RequestId,
               RequestDate = response.entity.RequestDate,
                Amount = response.entity.Amount,
                CurrencyCode = response.entity.Currency,
                Fees = response.entity.Fees,
                Status = response.entity.Status,
                CollectionDate = response.entity.CollectionDate,
                Notes = response.entity.Notes,
                IsBeneficiaryInitiated = response.entity.IsBeneficiaryInitiated,

            };
        }
    }

    public class ResposeEntity
    {
        public string? RequestId { get; set; }
        public DateTime RequestDate { get; set; }
        public decimal? Amount { get; set; } = 0.0m;
        public decimal? Fees { get; set; } = 0.0m;
        public string? Currency { get; set; }
        public string Status { get; set; }
        public bool IsBeneficiaryInitiated { get; set; }
        public string Notes { get; set; }
        public DateTime CollectionDate { get; set; }
        public Source source { get; set; }
        public Payer payer { get; set; }


    }
    public class Source
    {
        public string? OrganizationCode { get; set; }
        public string? AccountType { get; set; }
        public string? AccountId { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? ThirdName { get; set; }
        public string? LastName { get; set; }
        public string? Mobile { get; set; }
    }

    public class Payer
    {
        public string? organizationCode { get; set; }
        public string? accountType { get; set; }
        public string? accountId { get; set; }
    }

}
