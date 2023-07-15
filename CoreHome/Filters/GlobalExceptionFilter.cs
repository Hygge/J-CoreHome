using Common.Result;
using CoreHome.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoreHome.Filters
{
    public class GlobalExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }



        public Task OnExceptionAsync(ExceptionContext context)
        {
            _logger.LogInformation("异步异常捕获");
  
            switch (context.Exception)
            {
                case HyggeException:
                    var h = context.Exception as HyggeException;
                    context.Result = new ObjectResult(R.Fail(h.code, h.message));
                    break;
                default:
                    context.Result = new ObjectResult(R.Fail(424, context.Exception.Message));
                    break;
            }

            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }
    }
}
