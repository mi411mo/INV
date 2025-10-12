using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils.CustomException
{
    public class TharawatAccountingSysException : APIGatewayException
    {
        private const string exceptionType = "Accounting System Exception";
        //private const int exceptionErrorCode = 515;
        public TharawatAccountingSysException(int exceptionErrorCode) : base("An Exception has occured while communicating with the AccountingSystem")
        {
            Id = Generation.GenerateExceptionId();
            Type = exceptionType;
            HResult = exceptionErrorCode;
        }
        public TharawatAccountingSysException(string message, int exceptionErrorCode)
        : base(string.Format(message))
        {
            Id = Generation.GenerateExceptionId();
            Type = exceptionType;
            HResult = exceptionErrorCode;
        }

        public TharawatAccountingSysException(string message, Exception inner, int exceptionErrorCode) : base(message, inner)
        {
            Id = Generation.GenerateExceptionId();
            Type = exceptionType;
            HResult = exceptionErrorCode;
        }
        public TharawatAccountingSysException(string message, Exception inner) : base(message, inner)
        {
            Id = Generation.GenerateExceptionId();
            Type = exceptionType;
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
