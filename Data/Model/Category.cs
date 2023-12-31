﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Column(TypeName = "varchar(200)")]
        public string CategoryName { get; set; }

    }
}
