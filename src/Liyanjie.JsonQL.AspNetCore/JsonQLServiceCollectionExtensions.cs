using System;

using Liyanjie.JsonQL;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class JsonQLServiceCollectionExtensions
    {
        /// <summary>
        /// JsonQL
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddJsonQL(this IServiceCollection services, Action<JsonQLOptions> configureOptions)
        {
            services.Configure(configureOptions);
            services.AddSingleton<JsonQLMiddleware>();

            return services;
        }
    }
}