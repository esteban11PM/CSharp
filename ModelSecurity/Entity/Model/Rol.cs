using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Rol
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("active")]
        public bool Active { get; set; }
        [Column("description")]
        public string Description { get; set; }

        // Relaciones
        public ICollection<RolUser> RolUsers { get; set; } //Propiedad de navegacion inversa
        public ICollection<RolFormPermission> RolFormPermissions { get; set; } //Propiedad de navegacion inversa
    }
}
