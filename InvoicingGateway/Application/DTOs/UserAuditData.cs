using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UserAuditData
    {
        public string UserId { get; set; }
        public string ClientId { get; set; }
        public string CustomerId { get; set; }

        public UserAuditData(string userId, string clientId, string customerId)
        {
            UserId = userId;
            this.ClientId = clientId;
            this.CustomerId = customerId;
        }

        public UserAuditData()
        {
        }
    }
}
