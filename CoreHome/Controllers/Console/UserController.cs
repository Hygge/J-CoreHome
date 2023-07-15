using Common.Result;
using CoreHome.Filters;
using CoreHome.Utils;
using Data.Db;
using Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreHome.Controllers.Console
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly BlogDbContext _blogDbContext;

        public UserController(ILogger<UserController> logger, BlogDbContext blogDbContext)
        {
            this._logger = logger;
            this._blogDbContext = blogDbContext;

        }

        [TypeFilter(typeof(ConsoleFilter))]
        [HttpGet]
        public R GetUserInfo()
        { 
            Dictionary<string, object> result = new Dictionary<string, object>();
            User user = _blogDbContext.Users.First();
            user.Password = "";
            result.Add("userInfo", user);
            result.Add("setting", _blogDbContext.Settings.First());

            return R.Ok(result);
        }

        [TypeFilter(typeof(ConsoleFilter))]
        [Route("/api/User")]
        [HttpPatch]
        public R Patch(string? username, string? password, string? email)
        {
            int count = 0;
            var user = _blogDbContext.Users.First();
            if (!string.IsNullOrEmpty(username))
            {
                user.Name = username;
                count++;
            }
            if (!string.IsNullOrEmpty(password) && password.Replace(" ", "").Length > 6)
            {
                user.Password = SecurityUtil.SHA256Encrypt(password.Replace(" ", ""));
                count++;
            }
            if (!string.IsNullOrEmpty(email))
            {
                user.Email = email;
                count++;
            }
            if (count > 0)
            {
                _blogDbContext.SaveChanges();
            }        

            return R.Ok();
        }


    }
}
