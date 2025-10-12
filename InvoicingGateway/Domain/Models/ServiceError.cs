using Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ServiceError
    {
        /// <summary>
        /// CTOR
        /// </summary>
        /// 
        [NonSerialized]
        public IList<string> ErrorDetails;
        public const int THARAWAT_VALIDATION_ERROR_CODE = 4000;
        public ServiceError(string message, int code)
        {
            this.Message = message;
            this.Code = code;
        }

        public ServiceError() { }

        /// <summary>
        /// Human readable error message
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Machine readable error code
        /// </summary>
        public int Code { get; }

        /// <summary>
        /// Default error for when we receive an exception
        /// </summary>
        public static ServiceError DefaultError => new ServiceError("An exception occured.", 999);

        /// <summary>
        /// Use this to send a custom error message
        /// </summary>
        public static ServiceError CustomMessage(string errorMessage)
        {
            return new ServiceError(errorMessage, 997);
        }
        public static ServiceError ValidationError(string validationMessageError)
        {
            return new ServiceError(
                // developerMessage: validationMessageError,
                message: validationMessageError,
                code: THARAWAT_VALIDATION_ERROR_CODE);
        }
        public static ServiceError ServiceProvider => new ServiceError("Service Provider failed to return as expected.", 600);
        public static ServiceError ErrorOfReadAppsettings(string sectionName, string exceptionMessage = null)
        {
            return new ServiceError(
             //  developerMessage: $"Couldn't read {{{sectionName}}} or be empty or null in configuration file!",
             message: $"لم يتم التعرف على البيانات الخاصة بالخدمة ....يرجى التواصل مع الدعم الفني!",
             code: 5002);
        }

    }
}
