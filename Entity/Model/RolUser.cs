using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class RolUser
    {
        public int Id { get; set; }

        // Claves foráneas
        public int UserId { get; set; }
        public User User { get; set; }

        public int RoleId { get; set; }
        public Rol Rol { get; set; }
    }
    
}
