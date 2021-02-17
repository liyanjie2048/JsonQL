using Liyanjie.JsonQL.Tester;

namespace System.Web
{
    /// <summary>
    /// 
    /// </summary>
    public static class JsonQLTesterHttpApplicationExtensions
    {
        /// <summary>
        /// Add in Global.Application_Start (Use DI)
        /// </summary>
        /// <param name="app"></param>
        /// <param name="registerServiceType"></param>
        /// <param name="registerServiceInstance"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static HttpApplication AddJsonQLTester(this HttpApplication app,
            Action<Type, string> registerServiceType,
            Action<object, string> registerServiceInstance,
            Action<JsonQLTesterOptions> configureOptions = null)
        {
            var jsonQLTesterOptions = new JsonQLTesterOptions();
            configureOptions?.Invoke(jsonQLTesterOptions);

            registerServiceInstance.Invoke(jsonQLTesterOptions, "Singleton");
            registerServiceType.Invoke(typeof(JsonQLTesterSchemaMIddleware), "Singleton");
            registerServiceType.Invoke(typeof(JsonQLTesterRedirectMiddleware), "Singleton");
            registerServiceType.Invoke(typeof(JsonQLTesterEmbeddedMIddleware), "Singleton");

            return app;
        }

        /// <summary>
        /// Add in Global.Application_BeginRequest (Use DI)
        /// </summary>
        /// <param name="app"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="pathBase"></param>
        /// <returns></returns>
        public static HttpApplication UseJsonQLTester(this HttpApplication app,
            IServiceProvider serviceProvider,
            string pathBase = "jsonQL-tester")
        {
            (serviceProvider.GetService(typeof(JsonQLTesterSchemaMIddleware)) as JsonQLTesterSchemaMIddleware)
                ?.InvokeAsync(app.Context, pathBase)
                ?.Wait();
            (serviceProvider.GetService(typeof(JsonQLTesterRedirectMiddleware)) as JsonQLTesterRedirectMiddleware)
                ?.InvokeAsync(app.Context, pathBase)
                ?.Wait();
            (serviceProvider.GetService(typeof(JsonQLTesterEmbeddedMIddleware)) as JsonQLTesterEmbeddedMIddleware)
                ?.InvokeAsync(app.Context, pathBase)
                ?.Wait();

            return app;
        }

        #region Static Mode

        static JsonQLTesterOptions jsonQLTesterOptions;
        static JsonQLTesterSchemaMIddleware jsonQLTesterSchemaMIddleware;
        static JsonQLTesterRedirectMiddleware jsonQLTesterRedirectMiddleware;
        static JsonQLTesterEmbeddedMIddleware jsonQLTesterEmbeddedMIddleware;

        /// <summary>
        /// Add in Global.Application_Start (Static Mode)
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static HttpApplication AddJsonQLTester(this HttpApplication app,
            Action<JsonQLTesterOptions> configureOptions = null)
        {
            jsonQLTesterOptions = new JsonQLTesterOptions();
            configureOptions?.Invoke(jsonQLTesterOptions);

            jsonQLTesterSchemaMIddleware = new JsonQLTesterSchemaMIddleware(jsonQLTesterOptions);
            jsonQLTesterRedirectMiddleware = new JsonQLTesterRedirectMiddleware(jsonQLTesterOptions);
            jsonQLTesterEmbeddedMIddleware = new JsonQLTesterEmbeddedMIddleware(jsonQLTesterOptions);

            return app;
        }

        /// <summary>
        /// Add in Global.Application_BeginRequest (Static Mode)
        /// </summary>
        /// <param name="app"></param>
        /// <param name="pathBase"></param>
        /// <returns></returns>
        public static HttpApplication UseJsonQLTester(this HttpApplication app, 
            string pathBase = "jsonQL-tester")
        {
            jsonQLTesterSchemaMIddleware.InvokeAsync(app.Context, pathBase).Wait();
            jsonQLTesterRedirectMiddleware.InvokeAsync(app.Context, pathBase).Wait();
            jsonQLTesterEmbeddedMIddleware.InvokeAsync(app.Context, pathBase).Wait();

            return app;
        }

        #endregion
    }
}