using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils
{
    public class TokenObjectResponse
    {
        public bool Success { get; set; }
        public string ResponseCode { get; set; }
        public Entity entity { get; set; }

    }

    public class Entity
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
    }
}
