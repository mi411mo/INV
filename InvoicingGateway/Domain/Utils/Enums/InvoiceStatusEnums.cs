using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utils.Enums
{
    public enum InvoiceStatusEnums
    {
        [Description(" مسودة")]
        Draft = 1,
        [Description("غير مدفوعة")]
        Unpaid = 2,
        [Description("مدفوعة")]
        Paid = 3,
        [Description("مدفوعة جزئياً")]
        PartiallyPaid = 4,
        [Description("منتهية")]
        Expired = 5,
        [Description("ملغية")]
        Cancelled = 6,
    }
}
