using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utils.ResultUtils
{
    public class ResultAPI<R> : Result<R>
    {
        public string ResponseCode { get; protected set; }
        //public R Entity { get; protected set; }

        public ResultAPI<R> setResponseCode(string successCode)
        {
            this.ResponseCode = successCode;
            return this;
        }
        public ResultAPI<R> setMessage(string message)
        {
            this.Message = message;
            return this;
        }
        public ResultAPI<R> setEntity(R entity)
        {
            this.Entity = entity;
            return this;
        }
        public ResultAPI<R> setEntities(IList<R> entities)
        {
            this.Entities = entities;
            return this;
        }
    }
}
