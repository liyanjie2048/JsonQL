using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace Liyanjie.JsonQL.Tester
{
    public class JsonQLTesterEmbeddedMIddleware
    {
        readonly JsonQLTesterOptions options;

        public JsonQLTesterEmbeddedMIddleware(JsonQLTesterOptions options)
        {
            this.options = options;
        }

        public async Task InvokeAsync(HttpContext httpContext, string pathBase)
        {
            var embeddedPathBase = $"/{pathBase}/";
            var request = httpContext.Request;
            if (true
                && "GET".Equals(request.HttpMethod, StringComparison.OrdinalIgnoreCase)
                && request.Path.StartsWith(embeddedPathBase, StringComparison.OrdinalIgnoreCase))
            {
                using var stream = new JsonQLTesterVirtualPathProvider(typeof(Embedded).Assembly, Embedded.FileNamespace, embeddedPathBase)
                    .GetFile(httpContext.Request.Path)
                    .Open();
                using var reader = new StreamReader(stream);
                var content = await reader.ReadToEndAsync();

                var response = httpContext.Response;

                response.Clear();
                response.StatusCode = 200;
                response.Write(content);

                response.End();
            }
        }
    }
}
