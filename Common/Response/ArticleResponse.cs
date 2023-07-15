using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Response
{
    public  class ArticleResponse
    {
        public int ArId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 描述、概述
        /// </summary>
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

        public string CategoryName { get; set; }
        /// <summary>
        /// 是否发布
        /// </summary>
        public bool IsPublic { get; set; }

    }
}
