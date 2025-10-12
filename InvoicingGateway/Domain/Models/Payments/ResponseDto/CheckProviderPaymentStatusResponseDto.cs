using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Payments.ResponseDto
{
    public class CheckProviderPaymentStatusResponseDto
    {
        public string? RequestId { get; set; }
        public DateTime RequestDate { get; set; }
        public decimal? Amount { get; set; } = 0.0m;
        public decimal? Fees { get; set; } = 0.0m;
        public string? CurrencyCode { get; set; } 
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
