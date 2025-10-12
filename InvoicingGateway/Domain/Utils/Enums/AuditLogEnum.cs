using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utils.Enums
{
    public enum AuditLogEnum
    {
        None = 0,
        Create = 1,
        Update = 2,
        Delete = 3,
        UpdateStatus = 4
    }
}
