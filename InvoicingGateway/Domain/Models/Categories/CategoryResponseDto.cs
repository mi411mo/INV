using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Categories
{
    public class CategoryResponseDto
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
        public string UserId { get; set; }
        public string? CreatedBy { get; set; }
        public string? ClientId { get; set; }
        public string? CustomerId { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
