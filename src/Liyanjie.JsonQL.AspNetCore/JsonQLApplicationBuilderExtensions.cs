using System;

using Liyanjie.JsonQL;

using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 
    /// </summary>
    public static class JsonQLApplicationBuilderExtensions
    {
        /// <summary>
        /// JsonQL
        /// </summary>
        /// <param name="app"></param>
        /// <param name="routeTemplate"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseJsonQL(this IApplicationBuilder app,
            string routeTemplate = "jsonQL")
        {
            return app.MapWhen(context =>
            {
                var request = context.Request;
                if ("GET".Equals(request.Method, StringComparison.OrdinalIgnoreCase))
                {
                    var routeValues = new RouteValueDictionary();
                    var templateMatcher = new TemplateMatcher(TemplateParser.Parse(routeTemplate), routeValues);
                    return templateMatcher.TryMatch(request.Path, routeValues);
                }
                return false;
            }, _app => _app.UseMiddleware<JsonQLMiddleware>());
        }
    }
}