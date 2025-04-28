using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class RolFormPermissionDTO
    {
        public int Id { get; set; }
        public bool Active {get; set;}

        public int RolId { get; set; } // llave foranea de Rol
        public string RoleName { get; set; }

        public int PermissionId { get; set; } // llave foranea de Permission
        public string PermissionName { get; set; }

        public int FormId { get; set; } // llave foranea de Form
        public string FormName { get; set; }
    }
}
