using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class FormModule
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("active")]
        public bool Active { get; set; }

        [Column("formid")]
        public int FormId { get; set; }
        public Form Form { get; set; }

        [Column("moduleid")]
        public int ModuleId { get; set; }
        public Module Module { get; set; }
    }
}
