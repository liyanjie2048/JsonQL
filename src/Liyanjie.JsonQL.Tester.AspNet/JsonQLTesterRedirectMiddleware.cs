using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

using Liyanjie.TemplateMatching;

namespace Liyanjie.JsonQL.Tester
{
    public class JsonQLTesterRedirectMiddleware
    {
        readonly JsonQLTesterOptions options;

        public JsonQLTesterRedirectMiddleware(JsonQLTesterOptions options)
        {
            this.options = options;
        }

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
