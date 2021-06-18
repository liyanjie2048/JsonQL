using System;
using System.Linq;

namespace Liyanjie.JsonQL
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class JsonQLResource
    {
        /// <summary>
        /// 
        /// </summary>
        public abstract Type Type { get; }
        //public abstract bool Create(object obj);
        //public abstract bool Delete(IQueryable queryable, int count);
        //public abstract bool Update(IQueryable queryable, object obj);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonQLRequest"></param>
        /// <returns></returns>
        public abstract IQueryable BuildQuery(IJsonQLRequest jsonQLRequest);
    }
}
