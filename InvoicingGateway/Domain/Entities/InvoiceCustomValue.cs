using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("InvoiceCustomValues")]
    public class InvoiceCustomValue : ClientInfo
    {
        public long? Id { get; set; } = null;
        public int InvoiceId { get; set; }
        public int ParameterId { get; set; }
        public string ParameterValue { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } = null;
    }
}
