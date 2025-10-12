using Domain.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utils.ResultUtils
{
    public class ResultRepo<R> : Result<R>
    {


        public ReturnStatusEnum ReturnStatus { get; private set; } = ReturnStatusEnum.Success;
        //public string Message { get; private set; }
        //public IList<R> Entities { get; private set; }
        public object Id { get; private set; }
        public ResultRepo<R> setReturnStatus(ReturnStatusEnum returnStatus)
        {
            this.ReturnStatus = returnStatus;
            return this;
        }
        public ResultRepo<R> setMessage(string message)
        {
            this.Message = message;
            return this;
        }
        public ResultRepo<R> setReturnResponse(ReturnStatusEnum returnStatus, string message)
        {
            this.ReturnStatus = returnStatus;
            this.Message = message;
            return this;
        }
        public ResultRepo<R> setEntities(IList<R> entities)
        {
            this.Entities = entities;
            return this;
        }
        public ResultRepo<R> setEntity(R entity)
        {
            this.Entity = entity;
            return this;
        }

        public ResultRepo<R> setId(object id)
        {
            this.Id = id;
            return this;
        }
    }
}
