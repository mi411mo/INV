using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils.CustomException
{
    public class TharawatDBException : APIGatewayException
    {
        private const string exceptionType = "DataBase Exception";
        private const int exceptionErrorCode = DATABASE_SYSTEM_ERROR_CODE;
        public TharawatDBException() : base("DB Exception has occured")
        {
            Id = Generation.GenerateExceptionId();
            Type = exceptionType;
            HResult = exceptionErrorCode;
        }
        public TharawatDBException(string message)
        : base(string.Format(message))
        {
            Id = Generation.GenerateExceptionId();
            Type = exceptionType;
            HResult = exceptionErrorCode;
        }
        public TharawatDBException(string message, Exception inner) : base(message, inner)
        {
            Id = Generation.GenerateExceptionId();
            Type = exceptionType;
            HResult = exceptionErrorCode;
        }
        public static string ErrorCode { get; } = "500";
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Exception Id=" + Id + ", ")
            .Append("Exception Type=" + Type + ", ")
            .Append("Error code=" + HResult.ToString() + ", ")
            .Append("Message=" + Message + ", ")
            .Append("Root Exception=" + (InnerException != null ? InnerException.Message : null) + ".");
            return sb.ToString();
        }

        public const int DATABASE_SYSTEM_ERROR_CODE = 7503;
    }
}
