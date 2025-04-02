using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description {get; set;}
        public bool Active {get; set;}

        // Relación con RoleFormPermission
        public ICollection<RolFormPermission> RolFormPermissions { get; set; }
    }
}
