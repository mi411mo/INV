using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.AuditLogs
{
    public class AuditLogResponseDto
    {
        public int? Id { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }
        public string TableName { get; set; }
        public DateTime DateTime { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string ClientId { get; set; }
        public string CustomerId { get; set; }
        public string AffectedColumns { get; set; }
        public string PrimaryKey { get; set; }

        internal AuditLogResponseDto fromModel(AuditLog dto)
        {
            if (dto == null) return null;
            return new AuditLogResponseDto()
            {
                Id = dto.Id,
                UserId = dto.UserId,
                Type = dto.Type,
                TableName = dto.TableName,
                DateTime = dto.DateTime,
                OldValues = dto.OldValues,
                NewValues = dto.NewValues,
                AffectedColumns = dto.AffectedColumns,
                PrimaryKey = dto.PrimaryKey,
                ClientId = dto.ClientId,
                CustomerId = dto.CustomerId
            };
        }
        internal List<AuditLogResponseDto> fromModel(IList<AuditLog> dto)
        {
            var lst = new List<AuditLogResponseDto>();
            lst.AddRange(dto.Select((x) => fromModel(x)));
            return lst;
        }
    }
}
