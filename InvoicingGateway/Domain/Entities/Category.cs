using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("Categories")]
    public class Category : ClientInfo
    {
        public long? Id { get; set; } = null;       
        public string CategoryName { get; set; }
        public long MerchantId { get; set; }
        public string ImageURL { get; set; }
        public int? ProductCount { get; set; }
        public bool? IsActive { get; set; }
        public string Description { get; set; }
        public int? CategoryType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
