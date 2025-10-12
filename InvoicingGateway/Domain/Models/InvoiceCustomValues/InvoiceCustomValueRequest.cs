using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.InvoiceCustomValues
{
    public class InvoiceCustomValueRequest
    {
        public int ParameterId { get; set; }
        public string ParameterValue { get; set; }
    }
}
