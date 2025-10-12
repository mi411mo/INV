using Domain.Utils.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.InvoiceCustomParameters
{
    public class InvoiceParametersResponseDto
    {
        public long? Id { get; set; } = null;
        public int MerchantId { get; set; }
        public string ParameterName { get; set; }
        public InvoiceCustomParametersTypeEnum ParameterType { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } = null;
    }
}
