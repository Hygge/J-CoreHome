using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class FileInfom
    {
        [Key]
        public int Id { get; set; }

        public string FileName { get; set; }

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
        public string? Suffix { get; set; }

        public bool IsDelete { get; set; }
        public DateTime CreatedTime { get; set; }

        public long size { get; set; }


    }
}
