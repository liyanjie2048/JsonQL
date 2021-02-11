using System;
using System.Reflection;

using Liyanjie.JsonQL.Tester;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 
    /// </summary>
    public static class JsonQLTesterApplicationBuilderExtensions
    {
        /// <summary>
        /// JsonQL
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configue"></param>
        /// <param name="routeTemplate"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseJsonQLTester(this IApplicationBuilder app,
            string routeTemplate = "jsonQL-tester")
        {
            app.MapWhen(context =>
            {
                var request = context.Request;
                if ("GET".Equals(request.Method, StringComparison.OrdinalIgnoreCase))
                {
                    var routeValues = new RouteValueDictionary();
                    var templateMatcher = new TemplateMatcher(TemplateParser.Parse($"{routeTemplate}/schema.json"), routeValues);
                    return templateMatcher.TryMatch(request.Path, routeValues);
                }
                return false;
            }, _app => _app.UseMiddleware<JsonQLTesterSchemaMiddleware>());
            app.MapWhen(context =>
            {
                var request = context.Request;
                if ("GET".Equals(request.Method, StringComparison.OrdinalIgnoreCase))
                {
                    var routeValues = new RouteValueDictionary();
                    var templateMatcher = new TemplateMatcher(TemplateParser.Parse(routeTemplate), routeValues);
                    return templateMatcher.TryMatch(request.Path, routeValues);
                }
                return false;
            }, _app => _app.UseMiddleware<JsonQLTesterRedirectMiddleware>());

            var fileServerOptions = new FileServerOptions
            {
                RequestPath = $"/{routeTemplate}",
                EnableDefaultFiles = false,
                FileProvider = new EmbeddedFileProvider(typeof(Embedded).GetTypeInfo().Assembly, Embedded.FileNamespace),
            };
            app.UseFileServer(fileServerOptions);

            return app;
        }
    }
}