using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class RolUser
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("active")]
        public bool Active { get; set; }

        [Column("userid")]
        public int UserId { get; set; }
        public User User { get; set; }

        [Column("roleid")]
        public int RoleId { get; set; }
        public Rol Role { get; set; }
    }
    
}
