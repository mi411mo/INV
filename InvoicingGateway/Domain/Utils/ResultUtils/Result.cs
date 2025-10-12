using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utils.ResultUtils
{
    public class Result<R>
    {
        public string Message { get; protected set; }
        public IList<R> Entities { get; protected set; }
        public R Entity { get; protected set; }
    }
}
