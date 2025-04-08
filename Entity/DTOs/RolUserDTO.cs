using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class RolUserDTO
    {
        public int Id { get; set; }
        public bool Active {get; set;}

        public int UserId { get; set; } // LLave foranea de User
        public string UserName { get; set; }

        public int RoleId { get; set; } // LLave foranea de Rol
        public string RoleName { get; set; }
    }
}