#if NETCOREAPP3_0
using System.Reflection;

using Liyanjie.JsonQL.Tester;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.FileProviders;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 
    /// </summary>
    public static class JsonQLTesterEndpointRouteBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoints"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static IEndpointRouteBuilder MapJsonQLTester(this IEndpointRouteBuilder endpoints,
            string pattern = "/jsonQL-tester")
        {
            endpoints.MapGet($"{pattern}/schema.json", endpoints.CreateApplicationBuilder().UseMiddleware<JsonQLTesterSchemaMiddleware>().Build())
                .WithDisplayName("JsonQL Tester Schema");
            endpoints.MapGet(pattern, endpoints.CreateApplicationBuilder().UseMiddleware<JsonQLTesterRedirectMiddleware>().Build())
                .WithDisplayName("JsonQL Tester");
            endpoints.MapGet($"{pattern}/{{file}}", CreateRequestDelegate(endpoints, pattern))
                .WithDisplayName("JsonQL Tester Statics");

            return endpoints;
        }

        static RequestDelegate CreateRequestDelegate(IEndpointRouteBuilder endpoints, string pattern)
        {
            var app = endpoints.CreateApplicationBuilder();
            app.Use(next => context =>
            {
                // Set endpoint to null so the static files middleware will handle the request.
                context.SetEndpoint(null);
                return next(context);
            });
            app.UseFileServer(new FileServerOptions
            {
                RequestPath = pattern,
                EnableDefaultFiles = false,
                FileProvider = new EmbeddedFileProvider(typeof(Embedded).GetTypeInfo().Assembly, Embedded.FileNamespace),
            });
            return app.Build();
        }
    }
}
#endif
