using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace Liyanjie.JsonQL.Tester
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonQLTesterRedirectMiddleware : IMiddleware
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.Redirect($"{context.Request.Path}/index.html");

            await Task.CompletedTask;
        }
    }
}
