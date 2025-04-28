using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class User
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("State")]
        public bool State { get; set; }

        [Column("personid")]
        public int PersonId { get; set; }
        public Person Person { get; set; }

        public ICollection<RolUser> RolUsers { get; set; }
    }
}
