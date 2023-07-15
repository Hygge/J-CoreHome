using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string FUrl { get; set; }
        public string? FAvatar { get; set; }
        public string FUserName { get; set; }
        public string? FSign { get; set; }
        public bool IsDelete { get; set; }


    }
}
