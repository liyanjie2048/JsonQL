using System.Collections.Generic;

using Liyanjie.Linq.Expressions;

namespace Liyanjie.JsonQL
{
    /// <summary>
    /// 
    /// </summary>
    public class DynamicJsonQLEvaluator : IJsonQLEvaluator
    {
        /// <inheritdoc />
        public object Evaluate(string expression, ref IDictionary<string, object> variables)
        {
            return ExpressionEvaluator.Evaluate(expression, ref variables);
        }
    }
}