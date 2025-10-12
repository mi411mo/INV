using Domain.Models.Invoices.RequestDto;
using Domain.Models.Orders.RequestDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Orders.ResponseDto
{
    public class GetOrderResponseDto
    {
        public long? Id { get; set; } = null;
        public int MerchantId { get; set; }
        public decimal TotalAmount { get; set; }
        public string CurrencyCode { get; set; }
        public string OrderReference { get; set; }
        public List<ProductItems> Products { get; set; }
        public CustomerInfo CustomerInfo { get; set; }
        public int Status { get; set; }
        public int? CategoryType { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } = null;
    }
}
