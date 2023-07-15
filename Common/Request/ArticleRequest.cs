using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Request
{
    public class ArticleRequest : BasePage
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public int? CategoryId { get; set; }


    }
}
