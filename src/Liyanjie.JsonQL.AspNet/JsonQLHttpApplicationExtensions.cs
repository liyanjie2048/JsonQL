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
        /// <param name="registerServiceType"></param>
        /// <param name="registerServiceInstance"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static HttpApplication AddJsonQL(this HttpApplication app,
            Action<Type, string> registerServiceType,
            Action<object, string> registerServiceInstance,
            Action<JsonQLOptions> configureOptions = null)
        {
            var jsonQLOptions = new JsonQLOptions();
            configureOptions?.Invoke(jsonQLOptions);

            registerServiceInstance.Invoke(jsonQLOptions, "Singleton");
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
        static JsonQLMiddleware jsonQLMiddleware;

        /// <summary>
        /// Add in Global.Application_Start.(Static Mode)
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static HttpApplication AddJsonQL(this HttpApplication app, Action<JsonQLOptions> configureOptions = null)
        {
            jsonQLOptions = new JsonQLOptions();
            configureOptions?.Invoke(jsonQLOptions);
            jsonQLMiddleware = new JsonQLMiddleware(jsonQLOptions);

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
