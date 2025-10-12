using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utils.Enums
{
    public enum ProductsStatusEnum
    {
        [Description("مفعلة")]
        Active = 1,
        [Description("غير مفعلة")]
        Unactive = 2
    }
}
