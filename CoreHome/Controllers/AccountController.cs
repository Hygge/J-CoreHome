using Common.Result;
using CoreHome.Exceptions;
using CoreHome.Filters;
using CoreHome.Utils;
using Data.Db;
using Data.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CoreHome.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtHelper jwt;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<AccountController> _logger;
        private readonly BlogDbContext _blogDbContext;

        public AccountController(JwtHelper jwt, IMemoryCache memoryCache, ILogger<AccountController> logger, BlogDbContext blogDbContext)
        {
            this.jwt = jwt;
            this._memoryCache = memoryCache;
            this._logger = logger;
            this._blogDbContext = blogDbContext;

        }


        [HttpPost]
        public R Login( string username,  string password)
        {
         
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) 
            {
                throw new HyggeException(425, "参数异常");
            }
            int count = 0;
            string logError = "/account/login";
            if (_memoryCache.TryGetValue(logError, out count))
            {
                if (count > 5)
                {
                    //  + request.HttpContext.Connection.RemoteIpAddress?.ToString() 
                    _logger.LogError("密码输入错误5次，账号锁定24小时！IP: " + HttpContext.Connection.RemoteIpAddress?.ToString());
                    return R.Fail("密码输入错误5次，账号锁定24小时！");
                }
            }

            if (VerifyUserNameAndPassword(password))
            {
                // token头部信息没用到
                string token = jwt.CreateToken("hygge", 1);
                _memoryCache.Set("admin", token, TimeSpan.FromSeconds(60 * 5));                

                //Thread.Sleep(1500);

                return R.Ok(token);
            }      
         
             _memoryCache.Set(logError, ++count, TimeSpan.FromSeconds(60 * (60 * 12)));
            
                        
            return R.Fail("密码错错误！");
        }

        /// <summary>
        /// 校验登录密码
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool VerifyUserNameAndPassword(string password)
        {
            List<User> user = _blogDbContext.Users.ToList();
            return SecurityUtil.SHA256Encrypt(password).Equals(user[0].Password);
        }

        [TypeFilter(typeof(ConsoleFilter))]
        [HttpGet]
        public R LogOut()
        {
            _memoryCache.Remove("admin");
            _memoryCache.Remove("/account/login");
            return R.Ok("退出登录成功！");
        }

        [HttpGet]
        public R UserInfo()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            User user = _blogDbContext.Users.First();
            user.Password = "";
            result.Add("userInfo", user);
            result.Add("setting", _blogDbContext.Settings.First());
            return R.Ok(result);
        }
      

        [HttpGet]
        public R FriendsInfo()
        {
            List<FriendUrl> friendUrls = _blogDbContext.FriendUrls.Where(f => f.IsDelete == false).ToList();
      

            return R.Ok(friendUrls);
        }


    }
}
