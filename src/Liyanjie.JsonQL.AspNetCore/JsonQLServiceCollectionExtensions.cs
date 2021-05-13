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
        /// <param name="configureResources"></param>
        /// <returns></returns>
        public static IServiceCollection AddJsonQL(this IServiceCollection services,
            Action<JsonQLOptions> configureOptions,
            Func<IServiceProvider, JsonQLResourceTable> configureResources)
        {
            services.Configure(configureOptions);
            services.AddSingleton(configureResources);
            services.AddSingleton<JsonQLMiddleware>();

            return services;
        }
    }
}