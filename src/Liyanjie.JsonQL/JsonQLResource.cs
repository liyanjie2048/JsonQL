using System;
using System.Linq;

namespace Liyanjie.JsonQL
{
    public abstract class JsonQLResource
    {
        public abstract Type Type { get; }
        //public abstract bool Create(object obj);
        //public abstract bool Delete(IQueryable queryable, int count);
        //public abstract bool Update(IQueryable queryable, object obj);
        public abstract IQueryable BuildQuery(IJsonQLRequest jsonQLRequest);
    }
}
