using Domain.Utils.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Payments.RequestDto
{
    public class ConfirmPaymentRequestDto
    {
        public string RequestId { get; set; }
        public decimal Amount { get; set; } = 0;
        public string Currency { get; set; } = null;
        public string SourceProfileId { get; set; }
        public string SourceName { get; set; }
        public string SourcePhoneNumber { get; set; }
        public string TargetProfileId { get; set; }
        public string TargetName { get; set; }
        public string TargetPhoneNumber { get; set; }
        public string? PaymentMethod { get; set; }
        public string TransactionReference { get; set; }
        public string TargetReference { get; set; }
        public string PostUrl { get; set; }
        public TransactionStatusEnum TransactionStatus { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
