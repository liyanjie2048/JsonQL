using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

using Liyanjie.TemplateMatching;

namespace Liyanjie.JsonQL
{
    internal class JsonQLMiddleware
    {
        readonly JsonQLOptions jsonQLOptions;

        public JsonQLMiddleware(JsonQLOptions jsonQLOptions)
        {
            this.jsonQLOptions = jsonQLOptions ?? throw new ArgumentNullException(nameof(jsonQLOptions));
        }

        public async Task InvokeAsync(HttpContext httpContext, string routeTemplate)
        {
            var request = httpContext.Request;
            if ("GET".Equals(request.HttpMethod, StringComparison.OrdinalIgnoreCase))
            {
                var routeValues = new RouteValueDictionary();
                var templateMatcher = new TemplateMatcher(TemplateParser.Parse(routeTemplate), routeValues);
                if (templateMatcher.TryMatch(request.Path, routeValues))
                {
                    var response = httpContext.Response;
                    var jsonQLRequest = new JsonQLRequest(httpContext, jsonQLOptions);
                    var authorized = jsonQLOptions.AuthorizeAsync == null
                        ? true
                        : await jsonQLOptions.AuthorizeAsync(jsonQLRequest);
                    if (authorized)
                    {
                        var result = await new JsonQLHandler(jsonQLOptions).HandleAsync(jsonQLRequest);

                        response.Clear();
                        response.StatusCode = 200;
                        response.ContentType = "application/json";
                        response.Write(result);
                    }
                    else
                    {
                        response.StatusCode = 403;
                    }

                    response.End();
                }
            }
        }
    }
}
