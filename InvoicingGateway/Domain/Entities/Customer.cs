using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("Customers")]
    public class Customer : ClientInfo
    {
        public long? Id { get; set; } = null;
        public string Name { get; set; }
        public string ProfileId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; } = null;
        public int? CategoryType { get; set; }
        public int MerchantId { get; set; } 
        public string? InvoicePrefix { get; set; } = null;
        public string? Details { get; set; } = null;
        public bool? IsActive { get; set; }
        public DateTime CreatedAt {get; set;}

    }
}
