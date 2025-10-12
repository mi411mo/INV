using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PaymentMethodsResponse
    {
        public bool Success { get; set; }
        public string ResponseCode { get; set; }
        public string Message { get; set; }
        public IList<PaymentMethod> Entities { get; set; }
    }

    public class PaymentMethod
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string ImageUrl { get; set; }
        public string SectorType { get; set; }
    }
}
