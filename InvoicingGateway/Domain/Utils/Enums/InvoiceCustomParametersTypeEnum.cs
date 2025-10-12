using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utils.Enums
{
    public enum InvoiceCustomParametersTypeEnum
    {
        [Description(" نص ")]
        Text = 1,
        [Description(" عدد صحيح")]
        Integer = 2,
        [Description(" عدد عشري")]
        Decimal = 3,
        [Description("قيمة منطقية")]
        Bool = 4,
        [Description("تاريخ ووقت")]
        DateTime = 5
       
    }
}
