using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("Invoices")]
    public class InvoiceModel : ClientInfo
    {
        public long? Id { get; set; } = null;
        public int MerchantId { get; set; }
        public string InvoiceNumber { get; set; }
        public long PaymentToken { get; set; }
        public long? OrderId { get; set; }
        public string Customer { get; set; }
        public string Products { get; set; }
        public decimal TotalAmountDue { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal AmountRemaining { get; set; }
        public decimal AmountShipping { get; set; }
        public decimal Discount { get; set; }
        public string CurrencyCode { get; set; }
        public int? CategoryType { get; set; }
        public int Status { get; set; }
        public string CustomParameters { get; set; }
        public string AcceptedCurrencies { get; set; }
        public string PaymentMethods { get; set; }
        public string Notification { get; set; }
        public string Description { get; set; }
        public string OrderReference { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } = null;
    }
}
