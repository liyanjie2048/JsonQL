using System.Collections.Generic;

namespace Liyanjie.JsonQL
{
    /// <summary>
    /// 
    /// </summary>
    public interface IJsonQLEvaluator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        object Evaluate(string expression, ref IDictionary<string, object> variables);
    }
}
