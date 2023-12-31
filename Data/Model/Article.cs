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
    /// 文章
    /// </summary>
    public class Article
    {
        [Key]
        public int ArId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [Column(TypeName = "varchar(200)")]
        public string Title { get; set; }
        /// <summary>
        /// 描述、概述
        /// </summary>
         [Column(TypeName = "varchar(200)")]
        public string? Overview { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public int? CategoryId { get; set; }
        /// <summary>
        /// 浏览量
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// 是否发布
        /// </summary>
        public bool IsPublic { get; set; }
        /// <summary>
        ///  是否删除
        /// </summary>
        public bool IsDeleted { get; set; }







    }
}
