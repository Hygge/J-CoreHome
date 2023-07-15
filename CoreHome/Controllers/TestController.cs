using CoreHome.Filters;
using CoreHome.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CoreHome.Controllers
{


    [Route("[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly JwtHelper jwt;
        private readonly IMemoryCache _memoryCache;


        public TestController(JwtHelper jwt, IMemoryCache memoryCache)
        {
            this.jwt = jwt;
            _memoryCache = memoryCache;
        }

        [TypeFilter(typeof(ConsoleFilter))]
        [HttpGet]
        public string Index()
        {
            return "ok!";
        }

        [HttpGet]
        public string GetToken()
        {
            string str = jwt.CreateToken("hygge", 1);
            _memoryCache.Set("admin", str, TimeSpan.FromSeconds(60 * 5));
            return str;
        }
    }
}
