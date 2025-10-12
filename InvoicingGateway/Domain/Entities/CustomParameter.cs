using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("InvoiceCustomParameters")]
    public class CustomParameter
    {
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }
        public int ParameterType { get; set; }
    }
}
