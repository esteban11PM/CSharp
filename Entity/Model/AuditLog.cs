using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public int AffectedId { get; set; }
        public string PropertyName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }

        // Clave foránea con User
        public int UserId { get; set; }
        public User User { get; set; } //Propiedad de navegacion
    }
}
