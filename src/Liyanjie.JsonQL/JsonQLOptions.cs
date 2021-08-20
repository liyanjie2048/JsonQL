using System;
using System.Threading.Tasks;

namespace Liyanjie.JsonQL
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonQLOptions
    {
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
    }
}