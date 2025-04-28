using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class AuditLogDTO
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public int AffectedId { get; set; }
        public string PropertyName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
