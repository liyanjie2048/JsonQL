using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Liyanjie.JsonQL
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonQLMiddleware : IMiddleware
    {
        readonly JsonQLOptions jsonQLOptions;
        readonly JsonQLResourceTable jsonQLResourceTable;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonQLResourceTable"></param>
        /// <param name="jsonQLOptions"></param>
        public JsonQLMiddleware(
            IOptions<JsonQLOptions> jsonQLOptions,
            JsonQLResourceTable jsonQLResourceTable)
        {
            if (jsonQLOptions == null)
                throw new ArgumentNullException(nameof(jsonQLOptions));
            this.jsonQLOptions = jsonQLOptions.Value;
            this.jsonQLResourceTable = jsonQLResourceTable ?? throw new ArgumentNullException(nameof(jsonQLResourceTable));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var jsonQLRequest = new JsonQLRequest(context, jsonQLOptions);
            var authorized = jsonQLOptions.AuthorizeAsync == null
                ? true
                : await jsonQLOptions.AuthorizeAsync(jsonQLRequest);
            var response = context.Response;
            if (authorized)
            {
                var result = await new JsonQLHandler(jsonQLOptions, jsonQLResourceTable).HandleAsync(jsonQLRequest);

                response.Clear();
                response.StatusCode = 200;
                response.ContentType = "application/json";
                await response.WriteAsync(result);
            }
            else
            {
                response.StatusCode = 403;
            }
        }
    }
}
