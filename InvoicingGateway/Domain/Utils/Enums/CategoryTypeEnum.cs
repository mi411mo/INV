using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utils.Enums
{
    public enum CategoryTypeEnum
    {
        [Description(" الفواتير")]
        Invoices = 1,
        [Description(" الطلبات")]
        Orders = 2,
        [Description(" المدفوعات")]
        Payments = 3,
        [Description("التجار")]
        Merchants = 4,
        [Description("العملاء")]
        Customers = 5,
        [Description("المنتجات")]
        Products = 6,
        [Description("تصنيفات المنتجات")]
        Categories = 7,
        [Description("المطالبات المالية")]
        RequestPayment = 8
    }
}
