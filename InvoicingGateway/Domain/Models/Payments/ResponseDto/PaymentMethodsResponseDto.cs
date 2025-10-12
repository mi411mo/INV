using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Payments.ResponseDto
{
    public class PaymentMethodsResponseDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string ImageUrl { get; set; }
        public string SectorType { get; set; }
    }
}
