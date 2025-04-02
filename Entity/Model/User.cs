using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool State { get; set; }

        // Clave foránea con Person
        public int PersonId { get; set; }
        public Person Person { get; set; } // Propiedad de navegacion

        // Relaciones
        public ICollection<RolUser> RolUsers { get; set; } // Propiedad de navegacion inversa 
    }
}
