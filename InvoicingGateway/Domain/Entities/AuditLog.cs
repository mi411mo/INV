using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("AuditLogs")]
    public class AuditLog
   {
        public AuditLog(string userId, string type, string tableName, DateTime dateTime, string oldValues, string newValues, string affectedColumns, string primaryKey, string clientId, string customerId)
        {
            UserId = userId;
            Type = type;
            TableName = tableName;
            DateTime = dateTime;
            OldValues = oldValues;
            NewValues = newValues;
            AffectedColumns = affectedColumns;
            PrimaryKey = primaryKey;
            ClientId = clientId;
            CustomerId = customerId;
        }

        public int? Id { get; set; } = null;
        public string UserId { get; set; }
        public string Type { get; set; }
        public string TableName { get; set; }
        public DateTime DateTime { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string AffectedColumns { get; set; }
        public string PrimaryKey { get; set; }
        public string ClientId { get; set; }
        public string CustomerId { get; set; }

   }
}
