using System;

using Liyanjie.JsonQL.Tester;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class JsonQLTesterServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddJsonQLTester(this IServiceCollection services,
            Action<JsonQLTesterOptions> configureOptions = null)
        {
            if (configureOptions != null)
                services.Configure(configureOptions);

            services.AddSingleton<JsonQLTesterSchemaMiddleware>();
            services.AddSingleton<JsonQLTesterRedirectMiddleware>();

            return services;
        }
    }
}