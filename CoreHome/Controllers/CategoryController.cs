using Common.Result;
using CoreHome.Filters;
using Data.Db;
using Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreHome.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly BlogDbContext _blogDbContext;

        public CategoryController(ILogger<CategoryController> logger, BlogDbContext blogDbContext)
        {
            this._logger = logger;
            this._blogDbContext = blogDbContext;

        }

        [HttpGet]
        public R GetList()
        {

            return R.Ok(_blogDbContext.Categorys.ToList());
        }

        [TypeFilter(typeof(ConsoleFilter))]
        [HttpPost]
        public R Post(string categoryName)
        {
            _blogDbContext.Categorys.Add(new Category() { CategoryName = categoryName });
            _blogDbContext.SaveChanges();
            return R.Ok();
        }
        [TypeFilter(typeof(ConsoleFilter))]
        [Route("/api/Category/{id:int}")]
        [HttpDelete]
        public R Delete(int id)
        {
            _blogDbContext.Categorys.Remove(new Category() { CId = id });

            foreach (var article in _blogDbContext.Articles.Where(a => a.CategoryId == id))
            {
                article.CategoryId = null;
            }
            _blogDbContext.SaveChanges();
            return R.Ok();
        }
    }
}
