using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utils.Enums
{
    public enum IntegrationTypeEnum
    {
        [Description("ربط مباشر")]
        Direct = 1,
        [Description(" ربط غير مباشر")]
        Redirect = 2,
    }
}
