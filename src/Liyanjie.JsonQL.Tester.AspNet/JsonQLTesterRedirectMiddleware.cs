using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

using Liyanjie.TemplateMatching;

namespace Liyanjie.JsonQL.Tester
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonQLTesterRedirectMiddleware
    {
        readonly JsonQLTesterOptions options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public JsonQLTesterRedirectMiddleware(JsonQLTesterOptions options)
        {
            this.options = options;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="pathBase"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext, string pathBase)
        {
            var request = httpContext.Request;
            if ("GET".Equals(request.HttpMethod, StringComparison.OrdinalIgnoreCase))
            {
                var routeValues = new RouteValueDictionary();
                var templateMatcher = new TemplateMatcher(TemplateParser.Parse(pathBase), routeValues);
                if (templateMatcher.TryMatch(request.Path, routeValues))
                {
                    var response = httpContext.Response;
                    response.Redirect($"~/{pathBase}/index.html");
                    response.End();
                }
            }

            await Task.FromResult(0);
        }
    }
}
