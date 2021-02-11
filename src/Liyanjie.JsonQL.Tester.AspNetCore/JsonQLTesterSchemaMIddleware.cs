using System;
using System.Threading.Tasks;

using Liyanjie.JsonQL.Schema;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.Options;

namespace Liyanjie.JsonQL.Tester
{
    public class JsonQLTesterSchemaMiddleware : IMiddleware
    {
        readonly JsonQLTesterOptions jsonQLTesterOptions;

        public JsonQLTesterSchemaMiddleware(IOptions<JsonQLTesterOptions> options)
        {
            this.jsonQLTesterOptions = options.Value;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var schema = JsonQLSchema.Generate(jsonQLTesterOptions.ResourceTypes);
            var content = jsonQLTesterOptions.JsonSerialize(new
            {
                Title = jsonQLTesterOptions?.SchemaTitle,
                Description = jsonQLTesterOptions?.SchemaDescription,
                ServerUrl = jsonQLTesterOptions?.SchemaServerUrl,
                schema.ResourceInfos,
                schema.ResourceTypes,
                schema.ResourceMethods,
            });

            var response = context.Response;

            response.Clear();
            response.StatusCode = 200;
            response.ContentType = "application/json";
            await response.WriteAsync(content);
        }
    }
}
