using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class Setting
    {
        [Key]
        public int Id { get; set; }
        public string? Avatar { get; set; }
        public string? BgmUrl { get; set; }
        public string? ICPnumber { get; set; }

    }
}
