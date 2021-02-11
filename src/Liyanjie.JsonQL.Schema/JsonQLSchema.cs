using System;
using System.Collections.Generic;
using System.Linq;

namespace Liyanjie.JsonQL.Schema
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonQLSchema
    {
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<JsonQLSchemaResource> ResourceInfos { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<JsonQLSchemaClass> ResourceTypes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<JsonQLSchemaClass> ResourceMethods { get; set; }

        static readonly string nameOfResource = nameof(JsonQL.JsonQLQueryable);
        static readonly string nameOfResource_Ordered = nameof(JsonQLQueryable_Ordered);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static JsonQLSchema Generate(IDictionary<string, Type> resourceTypes)
        {
            return new JsonQLSchema
            {
                ResourceInfos = resourceTypes
                    .OrderBy(_ => _.Key)
                    .Select(_ => new JsonQLSchemaResource
                    {
                        Name = _.Key,
                        Type = _.Value.GetSchemaType(),
                    }),
                ResourceTypes = resourceTypes
                    .OrderBy(_ => _.Value.Name)
                    .Select(_ => new JsonQLSchemaClass
                    {
                        Name = _.Value.Name,
                        Properties = _.Value.GetSchemaProperties(),
                    }),
                ResourceMethods = new List<JsonQLSchemaClass>
                {
                    new JsonQLSchemaClass
                    {
                        Name = nameOfResource,
                        Methods = typeof(JsonQL.JsonQLQueryable).GetSchemaMethods(),
                    },
                    new JsonQLSchemaClass
                    {
                        Name = nameOfResource_Ordered,
                        Inherit = nameOfResource,
                        Methods = typeof(JsonQLQueryable_Ordered).GetSchemaMethods(),
                    },
                },
            };
        }
    }
}
