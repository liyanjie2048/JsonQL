using System;
using System.Collections.Generic;
using System.Linq;

namespace Liyanjie.JsonQL
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonQLResourceTable
    {
        readonly IDictionary<string, JsonQLResource> resources = new Dictionary<string, JsonQLResource>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IReadOnlyDictionary<string, Type> ResourceTypes => resources.ToDictionary(_ => _.Key, _ => _.Value.Type);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="jsonQLResource"></param>
        /// <returns></returns>
        public JsonQLResourceTable AddResource(
            string name,
            JsonQLResource jsonQLResource)
        {
            resources.Add(name, jsonQLResource);
            return this;
        }

        internal JsonQLResource GetResource(string template)
        {
            var name = template.Substring(0, template.IndexOf("[]"));

            if (resources.TryGetValue(name, out var resource))
                return resource;

            throw new Exception($"找不到名为“{name}”的资源");
        }
    }
}
