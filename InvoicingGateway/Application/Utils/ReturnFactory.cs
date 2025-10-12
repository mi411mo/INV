using Domain.Utils.ResultUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils
{
    public static class ReturnFactory
    {
        public static ResultAPI<R> ResultAPI<R>()
        {
            return new ResultAPI<R>();
        }
        /*public static ResultUC<R> ResultsUC<R>()
        {
            return new ResultUC<R>();
        }*/
        public static ResultRepo<R> ResultRepo<R>()
        {
            return new ResultRepo<R>();
        }
    }
}
