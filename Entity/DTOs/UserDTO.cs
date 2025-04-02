using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public bool State { get; set; }

        public int PersonId { get; set; } //Lave foranea
        public string PersonName { get; set; }
    }
}
