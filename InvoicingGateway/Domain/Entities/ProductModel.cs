using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("Products")]
    public class ProductModel : ClientInfo
    {
        public long? Id { get; set; } = null;
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public string CurrencyCode { get; set; }
        public string ImageURL { get; set; }
        public int CategoryId { get; set; }
        public int CategoryType { get; set; }
        public string SubCategoryId { get; set; }
        public int Status { get; set; }
        public int MerchantId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
