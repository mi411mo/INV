using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utils
{
    public class ValidationUtility
    {
        /// <summary>
        /// Check wither a give amount value is valid based on mobile type
        /// </summary>
        public static string ValidAmount(decimal amount)
        {
            // check it is not negative
            if (amount <= 0)
                return "المبلغ المدخل غير صحيح!";

            return string.Empty;
        }
        public static string ValidQuantity(decimal quantity)
        {
            // check it is not negative
            if (quantity <= 0)
                return "الكمية المدخلة غير صحيحة";

            return string.Empty;
        }
        public static string ValidPrice(decimal price)
        {
            // check it is not negative
            if (price <= 0)
                return "سعر الوحدة غير صحيح!";

            return string.Empty;
        }
    }
}
