using Common.Result;
using CoreHome.Filters;
using Data.Db;
using Data.Model;
using Microsoft.AspNetCore.Mvc;

namespace CoreHome.Controllers.Console
{

    [Route("[controller]/[action]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly BlogDbContext _blogDbContext;

        public SettingsController(ILogger<AccountController> logger, BlogDbContext blogDbContext)
        {
            this._logger = logger;
            this._blogDbContext = blogDbContext;

        }
      //  [TypeFilter(typeof(ConsoleFilter))]
        [HttpPost]
        public R AddSettings([FromBody] Dictionary<string, string> param)
        {
            if (param == null || param.Count() < 1)
            {
                return R.Ok();
            }
            using (var dbContext = _blogDbContext)
            {
                List<Blogsettings> blogs = new List<Blogsettings>();
                foreach (var item in param.Keys)
                {
                    Blogsettings blogsettings = dbContext.Blogsettingss.Where(b => b.Key.Equals(item)).FirstOrDefault();
                    if(blogsettings != null)
                    {
                        blogsettings.Value = param[item];
                        continue;
                    }
                    Blogsettings blogsettings1 = new Blogsettings();
                    blogsettings1.Value = param[item];
                    blogsettings1.Key = item;
                    blogs.Add(blogsettings1);
                }
                dbContext.Blogsettingss.AddRange(blogs);
                dbContext.SaveChanges();
            }
            return R.Ok();
        }

        [HttpGet]
        public R List()
        {
            Dictionary<string, string> keyValuePairs ;
            using (var dbContext = _blogDbContext)
            {
                List<Blogsettings> list = dbContext.Blogsettingss.ToList<Blogsettings>();
               /* foreach (var item in list)
                {
                    keyValuePairs.Add(item.Key, item.Value);
                }*/
                keyValuePairs = list.ToDictionary(x => x.Key, k => k.Value);
            }

                return R.Ok(keyValuePairs);
        }


    }
}
