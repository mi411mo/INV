using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils.CustomException
{
    public class APIGatewayException : Exception
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public APIGatewayException(string message)
        : base(message)
        { }
        public APIGatewayException(string message, Exception inner) : base(message, inner)
        { }
    }
}
