using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

using Liyanjie.JsonQL.Schema;
using Liyanjie.TemplateMatching;

namespace Liyanjie.JsonQL.Tester
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonQLTesterSchemaMIddleware
    {
        readonly JsonQLTesterOptions options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public JsonQLTesterSchemaMIddleware(JsonQLTesterOptions options)
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
                var templateMatcher = new TemplateMatcher(TemplateParser.Parse($"{pathBase}/schema.json"), routeValues);
                if (templateMatcher.TryMatch(request.Path, routeValues))
                {
                    var schema = JsonQLSchema.Generate(options.ResourceTypes);
                    var content = options.JsonSerialize(new
                    {
                        Title = options?.SchemaTitle,
                        Description = options?.SchemaDescription,
                        ServerUrl = options?.SchemaServerUrl,
                        schema.ResourceInfos,
                        schema.ResourceTypes,
                        schema.ResourceMethods,
                    });

                    var response = httpContext.Response;

                    response.Clear();
                    response.StatusCode = 200;
                    response.ContentType = "application/json";
                    response.Write(content);

                    response.End();
                }
            }

            await Task.FromResult(0);
        }
    }
}
