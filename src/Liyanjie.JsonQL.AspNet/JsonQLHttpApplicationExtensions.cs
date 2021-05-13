using Liyanjie.JsonQL;

namespace System.Web
{
    /// <summary>
    /// 
    /// </summary>
    public static class JsonQLHttpApplicationExtensions
    {
        /// <summary>
        /// Add in Global.Application_Start.(Use DI)
        /// </summary>
        /// <param name="app"></param>
        /// <param name="registerServiceInstance"></param>
        /// <param name="registerServiceFactory"></param>
        /// <param name="registerServiceType"></param>
        /// <param name="configureOptions"></param>
        /// <param name="configureResources"></param>
        /// <returns></returns>
        public static HttpApplication AddJsonQL(this HttpApplication app,
            Action<object, string> registerServiceInstance,
            Action<Func<IServiceProvider, object>, string> registerServiceFactory,
            Action<Type, string> registerServiceType,
            Action<JsonQLOptions> configureOptions,
            Func<IServiceProvider, JsonQLResourceTable> configureResources)
        {
            var jsonQLOptions = new JsonQLOptions();
            configureOptions?.Invoke(jsonQLOptions);

            registerServiceInstance.Invoke(jsonQLOptions, "Singleton");
            registerServiceFactory.Invoke(configureResources, "Singleton");
            registerServiceType.Invoke(typeof(JsonQLMiddleware), "Singleton");

            return app;
        }

        /// <summary>
        /// Add in Global.Application_BeginRequest.(Use DI)
        /// </summary>
        /// <param name="app"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="routeTemplate"></param>
        /// <returns></returns>
        public static HttpApplication UseJsonQL(this HttpApplication app,
            IServiceProvider serviceProvider,
            string routeTemplate = "jsonQL")
        {
            (serviceProvider.GetService(typeof(JsonQLMiddleware)) as JsonQLMiddleware)
                ?.InvokeAsync(app.Context, routeTemplate)
                ?.Wait();

            return app;
        }

        #region Static ModuleTable Mode

        static JsonQLOptions jsonQLOptions;
        static JsonQLResourceTable jsonQLResourceTable;
        static JsonQLMiddleware jsonQLMiddleware;

        /// <summary>
        /// Add in Global.Application_Start.(Static Mode)
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configureOptions"></param>
        /// <param name="configureResources"></param>
        /// <returns></returns>
        public static HttpApplication AddJsonQL(this HttpApplication app,
            Action<JsonQLOptions> configureOptions,
            Action<JsonQLResourceTable> configureResources)
        {
            jsonQLOptions = new JsonQLOptions();
            jsonQLResourceTable = new JsonQLResourceTable();
            configureOptions?.Invoke(jsonQLOptions);
            configureResources(jsonQLResourceTable);
            jsonQLMiddleware = new JsonQLMiddleware(jsonQLOptions, jsonQLResourceTable);

            return app;
        }

        /// <summary>
        /// Add in Global.Application_BeginRequest.(Static Mode)
        /// </summary>
        /// <param name="app"></param>
        /// <param name="routeTemplate"></param>
        /// <returns></returns>
        public static HttpApplication UseJsonQL(this HttpApplication app, string routeTemplate = "jsonQL")
        {
            jsonQLMiddleware.InvokeAsync(app.Context, routeTemplate).Wait();

            return app;
        }

        #endregion
    }
}
