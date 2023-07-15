using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreHome.Utils
{
    /// <summary>
    /// 分页工具类
    /// </summary>
    public class PageUtils<T> 
    {
        public int DataCount { get; set; } //总记录数
        public int PageCount { get; set; } //总页数
        public int PageNo { get; set; } //当前页码
        public int PageSize { get; set; } //每页显示记录数
        public List<T> List { get; set; }

    
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        public PageUtils(List<T> dataList, int pageSize, int pageNo, int count)
        {
            this.PageSize = pageSize;
            this.PageNo = pageNo;
            this.DataCount = count;
            this.PageCount = (int)Math.Ceiling((decimal)this.DataCount / pageSize);
            this.List = dataList;
           
        }


        public static int GetPage(int pageNo, int pageSize)
        {
            return (pageNo - 1) * pageSize;
        }
        
    }
}
