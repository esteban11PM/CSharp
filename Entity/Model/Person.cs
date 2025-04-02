using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Person
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string BlodType { get; set; }
        public bool Status { get; set; }

    }
}
