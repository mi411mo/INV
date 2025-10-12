using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utils.Enums
{
    public enum AccessChannelEnum
    {
        [Description(" ربط API")]
        API = 1,
        [Description("ويب")]
        Web = 2
    }
}
