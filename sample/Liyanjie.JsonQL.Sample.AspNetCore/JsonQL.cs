using System;
using System.Linq;
using System.Threading.Tasks;

namespace Liyanjie.JsonQL.Sample.AspNetCore
{
    class Resource<TEntity> : Liyanjie.JsonQL.JsonQLResource
    {
        readonly IQueryable<TEntity> queryable;
        public Resource(IQueryable<TEntity> queryable)
        {
            this.queryable = queryable;
        }

        public override Type Type => typeof(TEntity);

        public override IQueryable BuildQuery(Liyanjie.JsonQL.IJsonQLRequest jsonQLRequest)
        {
            return queryable;
        }
    }
}
