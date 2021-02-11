using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace Liyanjie.JsonQL
{
    internal class JsonQLRequest : IJsonQLRequest
    {
        readonly JsonQLOptions options;

        public JsonQLRequest(HttpContext httpContext, JsonQLOptions options)
        {
            this.HttpContext = httpContext;
            this.options = options;
        }

        public HttpContext HttpContext { get; private set; }

        public async Task<string> GetQueryAsync()
        {
            var request = HttpContext.Request;

            var query = options.FindQuery?.Invoke(this);
            if (query == null)
                query = request.Query["query"];
            if (query == null)
                query = request.Headers["query"];
            if (query == null)
            {
                using var reader = new StreamReader(request.Body);
                query = await reader.ReadToEndAsync();
            }

            return query;
        }
    }
}
