using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ErrorModel
    {
        public string ErrorCode { get; set; }
        public string DeveloperMessage { get; set; }
        public string UserMessage { get; set; }
        public string Details { get; set; }
    }
}
