using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class RolFormPermission
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("active")]
        public bool Active { get; set; }

        [Column("rolid")]
        public int RolId { get; set; }
        public Rol Rol { get; set; }

        [Column("permissionid")]
        public int PermissionId { get; set; }
        public Permission Permission { get; set; }

        [Column("formid")]
        public int FormId { get; set; }
        public Form Form { get; set; }
    }
}
