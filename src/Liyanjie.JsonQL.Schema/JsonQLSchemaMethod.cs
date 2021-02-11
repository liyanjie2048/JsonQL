using System.Collections.Generic;

namespace Liyanjie.JsonQL.Schema
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonQLSchemaMethod
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public JsonQLSchemaType ReturnType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<JsonQLSchemaParameter> Parameters { get; set; }
    }
}
