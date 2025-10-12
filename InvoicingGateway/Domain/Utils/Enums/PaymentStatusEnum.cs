using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utils.Enums
{
    public enum PaymentStatusEnum
    {
        [Description("ناجحة")]
        Success = 1,
        [Description("فاشلة")]
        Failure = 2,
        [Description("معلقة")]
        Pending = 3,
        [Description("بانتظار موافقة الدفع")]
        initiated = 4,
        [Description("منتهية")]
        Expired = 5,
        [Description("ملغية")]
        Cancelled = 6,
    }
}
