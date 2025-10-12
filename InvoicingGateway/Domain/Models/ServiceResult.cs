using Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ServiceResult<T> : ServiceResult
    {
        public bool Succeeded;
        public string ResponseCode { get; set; }
        public string Message { get; set; }
        ///// <value>The code for the response</value>

        public T Data { get; set; }

        public ServiceResult(T data)
        {
            Data = data;
        }
        public ServiceResult(ServiceError error) : base(error)
        {
            if (error == null)
            {
                error = ServiceError.DefaultError;
            }
            Error = error;
        }
    }
    public class ServiceResult
    {
        public bool Succeeded;
        public string Message { get; set; }
        /// <value>The code for the response</value>
        public string ResponseCode { get; set; }
        public ServiceError Error { get; set; }

        public ServiceResult(ServiceError error)
        {
            if (error == null)
            {
                error = ServiceError.DefaultError;
            }
            Error = error;
        }


        public ServiceResult() { }

        #region Helper Methods

        public static ServiceResult<T> Failed<T>(ServiceError error)
        {
            return new ServiceResult<T>(error);
        }
        public static ServiceResult<T> Success<T>(T data, string msg = Constants.SUCCESS_Message)
        {
            return new ServiceResult<T>(data)
            {

                Succeeded = true,
                ResponseCode = Constants.THARWAT_SUCCESS_CODE,
                Message = msg
            };
        }
        public static ServiceResult ResultFailed(ServiceError error)
        {
            return new ServiceResult()
            {
                Succeeded = false,
                Message = error.Message,
                ResponseCode = error.Code.ToString(),


            };
        }

        public static ServiceResult<R> ResultSuccess<R>(R data)//, string msg)
        {
            if (data != null)
                return new ServiceResult<R>(data)
                {
                    Succeeded = true,
                    ResponseCode = Constants.THARWAT_SUCCESS_CODE,
                    Message = Constants.SUCCESS_Message
                };
            return new ServiceResult<R>(data)
            {
                Message = "Data_Is_Empty"
            };
        }

        #endregion
    }
}
