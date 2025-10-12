using Domain.Utils.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.CategoryTypes
{
    public class CategoryTypeResponseDto
    {
        public long? Id { get; set; } = null;
        public string Name { get; set; }
        public string EnName { get; set; }
        public CategoryTypeEnum Type { get; set; }
        public string Code { get; set; }
        public bool? IsActive { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? ClientId { get; set; }
        public string? CustomerId { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
