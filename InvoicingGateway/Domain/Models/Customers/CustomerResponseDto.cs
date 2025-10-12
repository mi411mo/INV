using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Customers
{
    public class CustomerResponseDto
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public string ProfileId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; } = null;
        public int MerchantId { get; set; }
        public int? CategoryType { get; set; }
        public string? InvoicePrefix { get; set; } = null;
        public string? Details { get; set; } = null;
        public bool? IsActive { get; set; }
        
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
