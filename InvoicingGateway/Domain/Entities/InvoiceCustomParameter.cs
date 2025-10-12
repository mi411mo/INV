using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("InvoiceCustomParameters")]
    public class InvoiceCustomParameter : ClientInfo
    {
        public long? Id { get; set; } = null;
        public int MerchantId { get; set; }
        public string ParameterName { get; set; }
        public int ParameterType { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } = null;
    }
}
