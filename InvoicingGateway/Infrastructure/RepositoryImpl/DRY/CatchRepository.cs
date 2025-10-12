using Application.Utils;
using Domain.Utils.Enums;
using Domain.Utils.ResultUtils;
using System;

namespace IInfrastructure.RepositoryImpl.DRY
{
    public class CatchRepository : IExceptionLog
    {
        //DOTO: using Singleton Design Pattern
        public void Log(object obj)
        {
            
        }
        static string Resul(Exception ex)
        {
           new CatchRepository().Log(ex.Message);
            return "" + ex.Message;
        }
        public static ResultRepo<object> ResulCatchRepo(Exception ex)
        {
            Resul(ex);
            return ReturnFactory.ResultRepo<object>().setReturnStatus(ReturnStatusEnum.Exception).setMessage("ex="+ex);
        }
        public static ResultRepo<R> ResulCatchRepo<R>(Exception ex)
        {
            Resul(ex);
            return ReturnFactory.ResultRepo<R>().setReturnStatus(ReturnStatusEnum.Exception).setMessage("ex=" + ex);
        }

    }

    public interface IExceptionLog
    {
        public void Log(object obj);
    }
}
