using System;
using System.Threading.Tasks;

namespace Liyanjie.JsonQL
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonQLOptions
    {
        readonly JsonQLResourceTable resourceTable = new JsonQLResourceTable();

        /// <summary>
        /// 资源表
        /// </summary>
        public JsonQLResourceTable ResourceTable => resourceTable;

        /// <summary>
        /// 授权验证
        /// </summary>
        public Func<IJsonQLRequest, Task<bool>> AuthorizeAsync { get; set; } = context => Task.FromResult(true);

        /// <summary>
        /// 查找查询
        /// </summary>
        public Func<IJsonQLRequest, string> FindQuery { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IJsonQLIncluder JsonQLIncluder { internal get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IJsonQLEvaluator JsonQLEvaluator { internal get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IJsonQLLinqer JsonQLLinqer { internal get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<object, string> JsonSerialize { get; set; }
#if NET45
        = obj => Newtonsoft.Json.JsonConvert.SerializeObject(obj);
#endif
#if NETSTANDARD2_0
        = obj => System.Text.Json.JsonSerializer.Serialize(obj);
#endif

        /// <summary>
        /// 
        /// </summary>
        public Func<string, Type, object> JsonDeserialize { get; set; }
#if NET45
        = (str, type) => Newtonsoft.Json.JsonConvert.DeserializeObject(str, type);
#endif
#if NETSTANDARD2_0
        = (str, type) => System.Text.Json.JsonSerializer.Deserialize(str, type);
#endif
    }
}
