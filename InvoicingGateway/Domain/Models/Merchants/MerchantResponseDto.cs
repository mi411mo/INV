using Domain.Models.Base;
using Domain.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Merchants
{
    public class MerchantResponseDto
    {
        public long? Id { get; set; }
        public string ProfileId { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; } = null;
        public string InvoicePrefix { get; set; } = null;
        public int IntegrationType { get; set; }
        public int AccessChannel { get; set; }
        public string Details { get; set; } = null;
        public int CategoryType { get; set; }
        public bool? IsActive { get; set; }
        public string LogoImageUrl { get; set; }
        public string BusinessCategory { get; set; }
        public string BusinessDescription { get; set; }
        public string WebsiteUrl { get; set; }
        public Dictionary<string, string> SocialMedia { get; set; }
        public Location StoreLocation { get; set; }
        public Dictionary<string, string> OperatingHours { get; set; }
        public List<Review> CustomerReviews { get; set; }
        public string MarkdownContent { get; set; }
        public DateTime CreatedAt{ get; set; }
    }
}
