using Common.Result;
using CoreHome.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace CoreHome.Filters
{
    /// <summary>
    /// 后台权限拦截
    /// </summary>
    public class ConsoleFilter : Attribute, IAsyncAuthorizationFilter 
    {
        private readonly ILogger<ConsoleFilter> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly JwtHelper jwt;

        public ConsoleFilter(ILogger<ConsoleFilter> logger, JwtHelper jwtHelper, IMemoryCache memoryCache)
        {
            _logger = logger;
            jwt = jwtHelper;
            _memoryCache = memoryCache;
        }

        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
      

            string? token = context.HttpContext.Request.Headers["token"];
            _logger.LogInformation("权限拦截。。。。:" + token);
            if (string.IsNullOrEmpty(token))
            {              
                context.Result = new ObjectResult(R.Fail(401, "请先登录。"));
                return Task.CompletedTask;
            }
            // 校验是否登录
            if (IsAuthorized(token))
            {
                // 权限校验

                return Task.CompletedTask;
            }
            context.Result = new ObjectResult(R.Fail(401, "请先登录。"));
            return Task.CompletedTask;

        }


        public bool IsAuthorized(string token)
        {
            // 校验token合法性
            if (!jwt.IsToken(token))
            {
                _logger.LogError("token不合法");
                return false;
            }

            // 校验服务端是否存token        
            string? str = Convert.ToString(_memoryCache.Get("admin"));
            if(string.IsNullOrEmpty(str))
            {
                _logger.LogError("登录过期。");
                return false;
            }
            if(!str.Equals(token))
            {
                _logger.LogError("token不合法。");
                return false;
            }
            //_memoryCache.Set("admin", token, TimeSpan.FromSeconds(60 * 5));

            return true;
        }


    }
}
