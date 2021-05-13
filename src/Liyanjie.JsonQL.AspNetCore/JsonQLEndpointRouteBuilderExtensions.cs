using Liyanjie.JsonQL;

using Microsoft.AspNetCore.Routing;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 
    /// </summary>
    public static class JsonQLEndpointRouteBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoints"></param>
        /// <param name="pattern"></param>
        /// <param name="httpMethods"></param>
        /// <returns></returns>
        public static IEndpointRouteBuilder MapJsonQL(this IEndpointRouteBuilder endpoints,
            string pattern = "/jsonQL",
            params string[] httpMethods)
        {
            var pipeline = endpoints.CreateApplicationBuilder().UseMiddleware<JsonQLMiddleware>().Build();
            var displayName = "JsonQL";
            if (httpMethods == null)
                endpoints.Map(pattern, pipeline).WithDisplayName(displayName);
            else
                endpoints.MapMethods(pattern, httpMethods, pipeline).WithDisplayName(displayName);

            return endpoints;
        }
    }
}
