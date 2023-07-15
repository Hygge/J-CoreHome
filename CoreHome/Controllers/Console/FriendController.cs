using Common.Request;
using Common.Response;
using Common.Result;
using CoreHome.Filters;
using CoreHome.Utils;
using Data.Db;
using Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Xml.Linq;

namespace CoreHome.Controllers.Console
{
    [Route("[controller]")]
    [ApiController]
    public class FriendController : ControllerBase
    {

        private readonly ILogger<FriendController> _logger;
        private readonly BlogDbContext _blogDbContext;

        public FriendController(ILogger<FriendController> logger, BlogDbContext blogDbContext)
        {
            this._logger = logger;
            this._blogDbContext = blogDbContext;

        }

        [HttpGet]
        public R GetList(string? name, int current = 1, int size = 6)
        {
            int v = PageUtils<FriendUrl>.GetPage(current, size);        

            var query = _blogDbContext.FriendUrls.Where(f => f.IsDelete == false);
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(f => f.FUserName.StartsWith(name));
            }
            query.OrderByDescending(f => f.FId);
            var couant = query.Count();
            var p = new PageUtils<FriendUrl>(query.Skip(v).Take(size).ToList(), size, current, couant);
            return R.Ok(p);
        }
        [Route("/api/Friend/List")]
        [HttpGet]
        public R List()
        {
            var query = _blogDbContext.FriendUrls.Where(f => f.IsDelete == false).ToList();    
            List<FriendUrl> list = new List<FriendUrl>();
            Random Rand = new Random();
            foreach (var item in query)
            {
                list.Insert(Rand.Next(list.Count()), item);
            }

            return R.Ok(list);
        }

        [TypeFilter(typeof(ConsoleFilter))]
        [HttpPost]
        public R Post([FromBody] FriendUrl friend)
        {
            if (string.IsNullOrEmpty(friend.FUserName) || string.IsNullOrEmpty(friend.FUrl))
            {
                return R.Fail("名称和链接必填项");
            }
            FriendUrl friendUrl = new FriendUrl();
            friendUrl.FUserName = friend.FUserName;
            friendUrl.FUrl = friend.FUrl;
            friendUrl.FAvatar = friend.FAvatar;
            friendUrl.FSign = friend.FSign;

            _blogDbContext.FriendUrls.Add(friendUrl);
            _blogDbContext.SaveChanges();
            return R.Ok();
        }

        [TypeFilter(typeof(ConsoleFilter))]
        [Route("/api/Friend/{id:int}")]
        [HttpDelete]
        public R Delete(int id)
        {
            FriendUrl friend = _blogDbContext.FriendUrls.Where(f => f.FId == id).First();
            if(friend != null)
            {
                friend.IsDelete = true;
                _blogDbContext.SaveChanges();
            }              
            return R.Ok();
        }

        [TypeFilter(typeof(ConsoleFilter))]
        [Route("/api/Friend/{id:int}")]
        [HttpPatch]
        public R Patch(int id, [FromBody] FriendUrl friend)
        {
            if (friend == null)
            {
                return R.Fail("参数异常");
            }
            var article1 = _blogDbContext.FriendUrls.First(a => a.FId == id);
            if (!string.IsNullOrEmpty(friend.FUserName))
            {
                article1.FUserName = friend.FUserName;
            }
            if (!string.IsNullOrEmpty(friend.FSign))
            {
                article1.FSign = friend.FSign;
            }
            if (!string.IsNullOrEmpty(friend.FUrl))
            {
                article1.FUrl = friend.FUrl;
            }
            if (!string.IsNullOrEmpty(friend.FAvatar))
            {
                article1.FAvatar = friend.FAvatar;
            }         
            _blogDbContext.SaveChanges();

            return R.Ok();
        }

    }
}
