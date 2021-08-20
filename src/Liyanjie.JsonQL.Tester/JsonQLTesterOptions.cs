using System;
using System.Collections.Generic;
using System.Reflection;

namespace Liyanjie.JsonQL.Tester
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonQLTesterOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, Type> ResourceTypes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SchemaTitle { get; set; } = "JsonQL";

        /// <summary>
        /// 
        /// </summary>
        public string SchemaDescription { get; set; } = "Responsive Json Query Middleware";

        /// <summary>
        /// 
        /// </summary>
        public string SchemaServerUrl { get; set; } = "/jsonQL";
    }
}