using System.IO;
using System.Threading.Tasks;
using System.Web;

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

        public HttpContext HttpContext { get; }

        public async Task<string> GetQueryAsync()
        {
            var request = HttpContext.Request;

            var query = options.FindQuery?.Invoke(this);
            if (query == null)
                query = request.QueryString["query"];
            if (query == null)
                query = request.Headers["query"];
            if (query == null)
            {
                using var reader = new StreamReader(request.InputStream);
                query = await reader.ReadToEndAsync();
            }

            return query;
        }
    }
}
