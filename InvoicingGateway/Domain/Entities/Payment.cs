using Domain.Utils.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("Payments")]
    public class Payment : ClientInfo
    {
        public long? Id { get; set; }
        public string RequestId { get; set; } = null;
        public string TransactionReference { get; set; }
        public string TargetReference { get; set; }
        public string RequestPaymentReference { get; set; }
        public string InvoiceReference { get; set; }
        public string OrderReference { get; set; }
        public string PaymentReference { get; set; }
        public int MerchantId { get; set; }
        public string CustomerInfo { get; set; }
        public string PaymentMethod { get; set; }
        public PaymentStatusEnum PaymentStatus { get; set; }
        public PaymentTypeEnum PaymentType { get; set; }
        public decimal AmountDue { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? PayDate { get; set; }
        public string Details { get; set; }

    }
}
