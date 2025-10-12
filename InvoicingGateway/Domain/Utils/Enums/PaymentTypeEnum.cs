using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utils.Enums
{
    public enum PaymentTypeEnum
    {
        [Description(" الفواتير")]
        Invoices = 1,
        [Description(" الطلبات")]
        Orders = 2,
        [Description("المطالبات المالية")]
        RequestPayments = 3
    }
}
