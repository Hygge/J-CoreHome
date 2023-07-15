using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class Blogsettings
    {
        [Key]
        public int Id {  get; set; }
        [Column(TypeName = "varchar(200)")]
        public string Key { get; set; }
        public string Value { get; set; }

    }
}
