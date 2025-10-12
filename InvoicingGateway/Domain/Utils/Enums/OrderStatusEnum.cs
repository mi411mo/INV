using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utils.Enums
{
    public enum OrderStatusEnum
    {
        [Description("معلق")]
        Pending = 1,
        [Description("مصادق عليه")]
        Confirmed = 2,
        [Description("ملغي")]
        Cancelled = 3,
        [Description("منتهي")]
        Expired = 4,
        [Description("مدفوع")]
        Paid = 5,
    }
}
