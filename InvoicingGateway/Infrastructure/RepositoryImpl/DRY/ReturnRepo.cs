using Application.Utils;
using Domain.Utils.Enums;
using Domain.Utils.ResultUtils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories.Impl.V1.DRY
{
    public class ReturnRepo
    {
        public static ResultRepo<object> Success(object id = null)
        {
            //TODO: To be confirmed from this.
            if (id == null)
                return ReturnFactory.ResultRepo<object>().setReturnStatus(ReturnStatusEnum.Failed).setMessage("لا يوجد بيانات");
            return ReturnFactory.ResultRepo<object>().setId(id).setReturnStatus(ReturnStatusEnum.Success).setMessage("تمت العملية بنجاح");
        }

        public static ResultRepo<object> Success(bool isSucceed)
        {
            //TODO: To be confirmed from this.
            if (isSucceed)
                return ReturnFactory.ResultRepo<object>().setReturnStatus(ReturnStatusEnum.Success).setMessage("تمت العملية بنجاح");
            return ReturnFactory.ResultRepo<object>().setReturnStatus(ReturnStatusEnum.Failed).setMessage("لا يوجد بيانات");
        }
        public static ResultRepo<R> Success<R>(IList<R> lst)
        {
            if (lst.Count > 0)
                return ReturnFactory.ResultRepo<R>().setEntities(lst).setReturnStatus(ReturnStatusEnum.Success).setMessage("تمت العملية بنجاح");
            return ReturnFactory.ResultRepo<R>().setEntities(lst).setReturnStatus(ReturnStatusEnum.NotFound).setMessage("لا يوجد بيانات");
        }

    }
}
