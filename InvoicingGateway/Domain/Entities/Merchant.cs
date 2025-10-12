using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("Merchants")]
    public class Merchant : ClientInfo
    {
        public long? Id { get; set; } = null;
        public string ProfileId { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public string Phone { get; set; }
        public int CategoryType { get; set; }
        public int IntegrationType { get; set; }
        public int AccessChannel { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; } = null;
        public string? InvoicePrefix { get; set; } = null;
        public string? Details { get; set; } = null;
        public bool? IsActive { get; set; }
        public string LogoImageUrl { get; set; }
        public string BusinessCategory { get; set; }
        public string BusinessDescription { get; set; }
        public string WebsiteUrl { get; set; }
        public string SocialMedia { get; set; }
        public string StoreLocation { get; set; }
        public string OperatingHours { get; set; }
        public string CustomerReviews { get; set; }
        public string MarkdownContent { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CustomerId { get; set; }

    }
}
