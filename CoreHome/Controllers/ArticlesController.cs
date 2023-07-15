
using Common.Request;
using Common.Response;
using Common.Result;
using CoreHome.Filters;
using CoreHome.Utils;
using Data.Db;
using Data.Model;
using Microsoft.AspNetCore.Mvc;


namespace CoreHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly BlogDbContext _blogDbContext;

        public ArticlesController(ILogger<AccountController> logger, BlogDbContext blogDbContext)
        {     
            this._logger = logger;
            this._blogDbContext = blogDbContext;

        }

        [TypeFilter(typeof(ConsoleFilter))]
        [HttpPost]
        public R List([FromBody] ArticleRequest articleRequest)
        {
            int v = PageUtils<Article>.GetPage(articleRequest.current, articleRequest.pageSize);
            var query = from a in _blogDbContext.Set<Article>()
                        join c in _blogDbContext.Set<Category>()
                            on a.CategoryId equals c.CId into grouping                           
                        from c in grouping.DefaultIfEmpty()
                        where a.IsDeleted == false 
                        select new ArticleResponse(){ ArId=a.ArId, Title = a.Title, Number = a.Number, Overview = a.Overview,
                            CreatedTime = a.CreatedTime, CategoryId = a.CategoryId, CategoryName = c.CategoryName, IsPublic = a.IsPublic };       
  
            if (articleRequest.CategoryId != null && articleRequest.CategoryId != 0)
            {
                /*   query = from a in _blogDbContext.Set<Article>()
                           join c in _blogDbContext.Set<Category>()
                               on a.CategoryId equals c.CId into grouping
                           from c in grouping.DefaultIfEmpty()
                           where a.IsDeleted == false
                           where a.CategoryId == articleRequest.CategoryId
                           select new ArticleResponse()
                           {
                               ArId = a.ArId,
                               Title = a.Title,
                               Number = a.Number,
                               Overview = a.Overview,
                               CreatedTime = a.CreatedTime,
                               CategoryId = a.CategoryId,
                               CategoryName = c.CategoryName,
                               IsPublic = a.IsPublic
                           };*/
                query = query.Where(a => a.CategoryId == articleRequest.CategoryId);
            }
             if (!string.IsNullOrEmpty(articleRequest.Title))
            {
                /*             query = from a in _blogDbContext.Set<Article>()
                                     join c in _blogDbContext.Set<Category>()
                                         on a.CategoryId equals c.CId into grouping
                                     from c in grouping.DefaultIfEmpty()
                                     where a.IsDeleted == false 
                                     where a.Title.StartsWith(articleRequest.Title)
                                     select new ArticleResponse()
                                     {
                                         ArId = a.ArId,
                                         Title = a.Title,
                                         Number = a.Number,
                                         Overview = a.Overview,
                                         CreatedTime = a.CreatedTime,
                                         CategoryId = a.CategoryId,
                                         CategoryName = c.CategoryName,
                                         IsPublic = a.IsPublic
                                     };*/
                query = query.Where(a => a.Title.StartsWith(articleRequest.Title));
            }
            int count = query.Count();        
            return R.Ok(new PageUtils<ArticleResponse>(query.OrderByDescending(c => c.CreatedTime).Skip(v).Take(articleRequest.pageSize).ToList(),
                articleRequest.pageSize, articleRequest.current, count));
        }

        [TypeFilter(typeof(ConsoleFilter))]
        [HttpPut]
        public R Put([FromBody] Article article)
        {
            if (article == null || string.IsNullOrEmpty(article.Title) || string.IsNullOrEmpty(article.Overview) 
                || string.IsNullOrEmpty(article.Content))
            {
                return R.Fail("参数异常");
            }           
            article.CreatedTime = DateTime.Now;
            article.Number = 0;
            article.IsPublic = true;
            article.IsDeleted = false;
            _blogDbContext.Articles.Add(article);
            _blogDbContext.SaveChanges();          

            return R.Ok();
        }

        [TypeFilter(typeof(ConsoleFilter))]
        [Route("/api/Articles/{id:int}")]
        [HttpPatch]
        public R Patch(int id, [FromBody] Article article)
        {
            if (article == null)
            {
                return R.Fail("参数异常");
            }

            var article1 = _blogDbContext.Articles.First(a => a.ArId == id);
            if (!string.IsNullOrEmpty(article.Title))
            {
                article1.Title = article.Title;
            }
            if (!string.IsNullOrEmpty(article.Overview))
            {
                article1.Overview = article.Overview;
            }
            if (!string.IsNullOrEmpty(article.Content))
            {
                article1.Content = article.Content;
            }  
            if(article.CategoryId != 0 )
            {
                article1.CategoryId = article.CategoryId;
            }
            article1.IsPublic = article.IsPublic;        

           // _blogDbContext.Articles.Update(article1);
            _blogDbContext.SaveChanges();     
          
            return R.Ok();
        }

        [TypeFilter(typeof(ConsoleFilter))]
        [Route("/api/Articles/{id:int}")]
        [HttpDelete ]
        public R Delete(int id)
        {
            var article1 = _blogDbContext.Articles.First(a => a.ArId == id);
            article1.IsDeleted = true;
            _blogDbContext.SaveChanges();
            return R.Ok();
        }


        [Route("/api/Articles/{id:int}")]
        [HttpGet]
        public R Get(int id)
        {
            var article = _blogDbContext.Articles.Where(a => a.ArId == id && a.IsDeleted == false).First();         
            return R.Ok(article);
        }
        [Route("/api/Articles/ListA")]
        [HttpPost]
        public R ListA([FromBody] ArticleRequest articleRequest)
        {
            int v = PageUtils<Article>.GetPage(articleRequest.current, articleRequest.pageSize);
            var query = from a in _blogDbContext.Set<Article>()
                        join c in _blogDbContext.Set<Category>()
                            on a.CategoryId equals c.CId into grouping
                        from c in grouping.DefaultIfEmpty()
                        where a.IsDeleted == false
                        where a.IsPublic == true
                        select new ArticleResponse()
                        {
                            ArId = a.ArId,
                            Title = a.Title,
                            Number = a.Number,
                            Overview = a.Overview,
                            CreatedTime = a.CreatedTime,
                            CategoryId = a.CategoryId,
                            CategoryName = c.CategoryName,
                            IsPublic = a.IsPublic
                        };

            if (articleRequest.CategoryId != null && articleRequest.CategoryId != 0)
            {        
                query = query.Where(a => a.CategoryId == articleRequest.CategoryId);
            }
            if (!string.IsNullOrEmpty(articleRequest.Title))
            {
    
                query = query.Where(a => a.Title.StartsWith(articleRequest.Title));
            }
            int count = query.Count();
            return R.Ok(new PageUtils<ArticleResponse>(query.OrderByDescending(c => c.CreatedTime).Skip(v).Take(articleRequest.pageSize).ToList(),
                articleRequest.pageSize, articleRequest.current, count));
        }

    }
}
