using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class FileInfom
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(200)")]

        public string FileName { get; set; }
        [Column(TypeName = "varchar(255)")]

        public string? url { get; set; }
       
        /// <summary>
        /// 是否本地
        /// </summary>
        public bool local { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 文件后缀
        /// </summary>
        [Column(TypeName = "varchar(10)")]
        public string? Suffix { get; set; }

        public bool IsDelete { get; set; }
        public DateTime CreatedTime { get; set; }

        public long size { get; set; }


    }
}
