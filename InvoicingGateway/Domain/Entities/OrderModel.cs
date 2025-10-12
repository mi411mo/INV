using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("Orders")]
    public class OrderModel : ClientInfo
    {
        public long? Id { get; set; } = null;
        public int MerchantId { get; set; }
        public decimal TotalAmount { get; set; }
        public string CurrencyCode { get; set; }
        public string OrderReference { get; set; }
        public string Products { get; set; }
        public string CustomerInfo { get; set; }
        public int? CategoryType { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } = null;
    }
}
