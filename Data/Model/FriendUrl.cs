using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    /// <summary>
    /// 友链
    /// </summary>
    public class FriendUrl
    {
        [Key]
        public int FId { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string FUrl { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string? FAvatar { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string FUserName { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string? FSign { get; set; }
        public bool IsDelete { get; set; }


    }
}
