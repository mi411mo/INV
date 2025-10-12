using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utils.Enums
{
    public enum ServiceTypeEnum
    {
        [Description("Public")]
        Public = 0,
        [Description("Transfer")]
        Transfer = 1,
        [Description("Payment")]
        Payment = 2,
        [Description("Accounting")]
        Accounting = 3,
        [Description("Wallets")]
        Wallets = 4,
        [Description("Banks")]
        Banks = 5,
        [Description("Purchases")]
        Purchases = 6,
        [Description("Fatora")]
        Fatora = 7
    }
}
