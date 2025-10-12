using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("Logs")]
    public class Log : ClientInfo
    {
        public long? Id { get; set; } = null;
        public DateTime? LogTime { get; set; } = null;
        public string LogType { get; set; }
        public string RequestId { get; set; }
        public string SourceId { get; set; }
        public string TargetId { get; set; }
        public string ServiceId { get; set; }
        public string Content { get; set; }
        public string Details { get; set; }
    }
}
