using System.Collections.Generic;

namespace Liyanjie.JsonQL.Schema
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonQLSchemaClass
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Inherit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<JsonQLSchemaProperty> Properties { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<JsonQLSchemaMethod> Methods { get; set; }
    }
}
