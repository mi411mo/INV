using Domain.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class RestAPIGenericResponseDTO<T>
    {
        public bool Success { get; set; }
        public string ResponseCode { get; set; }
        public string Message { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public T Entity { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public IList<T> Entities { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public List<ErrorModel> Errors { get; set; } = null;
        public RestAPIGenericResponseDTO() { }

        /// <summary>
        /// This method build a new instance indicating error state
        /// </summary>
        public RestAPIGenericResponseDTO<T> WithError(object errCode, string errMsg)
        {
            Success = false;
            ResponseCode = errCode.ToString();
            Message = errMsg;

            return this;
        }
        public RestAPIGenericResponseDTO<T> WithSuccess(object resCode, string msg)
        {
            Success = true;
            ResponseCode = resCode.ToString();
            Message = msg;


            return this;
        }

        /// <summary>
        /// This method build a new instance indicating success state
        /// </summary>
        public RestAPIGenericResponseDTO<T> WithSuccess(object resCode, string msg, T entity)
        {
            Success = true;
            ResponseCode = resCode.ToString();
            Message = msg;
            Entity = entity;


            return this;
        }

        /// <summary>
        /// This method build a new instance indicating success state
        /// </summary>
        public RestAPIGenericResponseDTO<T> WithPending(object resCode, string msg, T entity)
        {
            Success = true;
            ResponseCode = resCode.ToString();
            Message = msg;
            Entity = entity;

            return this;
        }

        /// <summary>
        /// This method build a new instance indicating success state
        /// </summary>
        public RestAPIGenericResponseDTO<T> WithSuccess(object resCode, string msg, IList<T> entity)
        {
            Success = true;
            ResponseCode = resCode.ToString();
            Message = msg;
            Entities = entity;

            return this;
        }
        /// <summary>
        /// This method build a new instance indicating Exception state
        /// </summary>
        public RestAPIGenericResponseDTO<T> WithException(Exception ex)
        {
            Success = false;
            ResponseCode = "7503";
            Message = ex.Message;

            return this;
        }
        public RestAPIGenericResponseDTO<T> WithSqlException(SqlException ex)
        {
            Success = false;
            ResponseCode = Constants.API_DATABASE_SYSTEM_ERROR_CODE;
            Message = ex.Message;
            //Message = Constants.GeneralUserErrorMessage;

            return this;
        }
        public RestAPIGenericResponseDTO<T> WithForeignKeyViolationException(SqlException ex, string msg)
        {
            Success = false;
            ResponseCode = Constants.API_DATABASE_SYSTEM_ERROR_CODE;
            Message = msg;

            return this;
        }

        public RestAPIGenericResponseDTO<T> WithJSONRequestValidationError(List<ErrorModel> errors)
        {
            Success = false;
            ResponseCode = Constants.CLIENT_ERROR_CODE;
            Message = Constants.GenericErrorMessage;
            Errors = errors;

            return this;
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
