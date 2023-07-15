using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    /// <summary>
    /// 分类
    /// </summary>
    public class Category
    {
        [Key]
        public int CId { get; set; }
        /// <summary>
        /// 类名
        /// </summary>
        public string CategoryName { get; set; }

    }
}
