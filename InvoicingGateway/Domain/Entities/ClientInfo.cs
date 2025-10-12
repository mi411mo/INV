using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ClientInfo
    {
        public string UserId { get; set; }
        public string? CreatedBy { get; set; }
        public string ClientId { get; set; }
        public string CustomerId { get; set; }
    }
}
