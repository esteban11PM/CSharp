using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Form
    {
        [Column("id")] // id en snake_case para postgresql
        public int Id { get; set; }

        [Column("name")] // name en snake_case para postgresql
        public string Name { get; set; }

        [Column("active")] // active en snake_case para postgresql
        public bool Active { get; set; }

        [Column("description")] // description en snake_case para postgresql
        public string Description { get; set; }

        // Relaciones
        public ICollection<RolFormPermission> RolFormPermissions { get; set; } //Propiedad de navegacion inversa
        public ICollection<FormModule> FormModules { get; set; } //Propiedad de navegacion inversa

    }
}
