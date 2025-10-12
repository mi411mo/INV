using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Services
{
    public class ProductResponseDto
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public int CategoryType { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public int MerchantId { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public string CurrencyCode { get; set; }
        public string ImageURL { get; set; }
        public int CategoryId { get; set; }
        public string SubCategoryId { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
