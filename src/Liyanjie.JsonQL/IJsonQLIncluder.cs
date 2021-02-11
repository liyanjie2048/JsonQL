using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liyanjie.JsonQL
{
    /// <summary>
    /// 
    /// </summary>
    public interface IJsonQLIncluder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="navigationPropertyPath"></param>
        /// <returns></returns>
        IQueryable Include(IQueryable queryable, params string[] navigationPropertyPath);
    }
}
