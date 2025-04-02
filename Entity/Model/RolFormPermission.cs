using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class RolFormPermission
    {
        public int Id { get; set; }

        // Claves foráneas
        public int RoleId { get; set; } 
        public Rol Rol { get; set; } //Propiedad de navegacion  

        public int PermissionId { get; set; }
        public Permission Permission { get; set; } //Propiedad de navegacion

        public int FormId { get; set; }
        public Form Form { get; set; } //Propiedad de navegacion
    }
}
