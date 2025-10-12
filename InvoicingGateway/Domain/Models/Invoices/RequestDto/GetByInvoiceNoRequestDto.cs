using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace Domain.Models.Invoices.RequestDto
{
    public class GetByInvoiceNoRequestDto
    {
        [Required(ErrorMessage = "InvoiceNumber is required"), MaxLength(30), MinLength(3)]
        public string InvoiceNumber { get; set; }
    }
}
