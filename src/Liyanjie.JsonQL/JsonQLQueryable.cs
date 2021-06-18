using System.Collections.Generic;
using System.Linq;

namespace Liyanjie.JsonQL
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonQLQueryable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="jsonQLIncluder"></param>
        /// <param name="jsonQLLinqer"></param>
        public JsonQLQueryable(
            IQueryable queryable,
            IJsonQLIncluder jsonQLIncluder,
            IJsonQLLinqer jsonQLLinqer)
        {
            Queryable = queryable;
            JsonQLIncluder = jsonQLIncluder;
            JsonQLLinqer = jsonQLLinqer;
        }

        internal JsonQLResource JsonQLResource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        internal IQueryable Queryable { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        internal protected readonly IJsonQLIncluder JsonQLIncluder;

        /// <summary>
        /// 
        /// </summary>
        internal protected readonly IJsonQLLinqer JsonQLLinqer;

        /// <summary>
        /// 
        /// </summary>
        protected IDictionary<string, object> Parameters { get; private set; }

        internal JsonQLQueryable SetParameters(IDictionary<string, object> parameters)
        {
            this.Parameters = parameters;
            return this;
        }

        internal JsonQLQueryable Include(params string[] paths)
        {
            if (JsonQLIncluder != null)
                Queryable = JsonQLIncluder.Include(Queryable, paths);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool All(string predicate)
        {
            return JsonQLLinqer.All(Queryable, predicate, Parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool Any(string predicate = null)
        {
            if (predicate == null)
                return JsonQLLinqer.Any(Queryable);
            else
                return JsonQLLinqer.Any(Queryable, predicate, Parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object Average()
        {
            return JsonQLLinqer.Average(Queryable);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public object Average(string selector)
        {
            return JsonQLLinqer.Average(Queryable, selector, Parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int Count(string predicate = null)
        {
            if (predicate == null)
                return JsonQLLinqer.Count(Queryable);
            else
                return JsonQLLinqer.Count(Queryable, predicate, Parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonQLQueryable Distinct()
        {
            Queryable = JsonQLLinqer.Distinct(Queryable);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public JsonQLQueryable GroupBy(string selector)
        {
            Queryable = JsonQLLinqer.GroupBy(Queryable, selector, Parameters);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object Max()
        {
            return JsonQLLinqer.Max(Queryable);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public object Max(string selector)
        {
            return JsonQLLinqer.Max(Queryable, selector, Parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object Min()
        {
            return JsonQLLinqer.Min(Queryable);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public object Min(string selector)
        {
            return JsonQLLinqer.Min(Queryable, selector, Parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public JsonQLQueryable_Ordered OrderBy(string selector)
        {
            return new JsonQLQueryable_Ordered(this, JsonQLLinqer.OrderBy(Queryable, selector, Parameters));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public JsonQLQueryable_Ordered OrderByDescending(string selector)
        {
            return new JsonQLQueryable_Ordered(this, JsonQLLinqer.OrderByDescending(Queryable, selector, Parameters));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public JsonQLQueryable Select(string selector)
        {
            Queryable = JsonQLLinqer.Select(Queryable, selector, Parameters);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public JsonQLQueryable Skip(int count)
        {
            Queryable = JsonQLLinqer.Skip(Queryable, count);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object Sum()
        {
            return JsonQLLinqer.Sum(Queryable);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public object Sum(string selector)
        {
            return JsonQLLinqer.Sum(Queryable, selector, Parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public JsonQLQueryable Take(int count)
        {
            Queryable = JsonQLLinqer.Take(Queryable, count);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public JsonQLQueryable Where(string predicate)
        {
            Queryable = JsonQLLinqer.Where(Queryable, predicate, Parameters);
            return this;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class JsonQLQueryable_Ordered : JsonQLQueryable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonQLQueryable"></param>
        /// <param name="orderedQueryable"></param>
        public JsonQLQueryable_Ordered(
            JsonQLQueryable jsonQLQueryable,
            IOrderedQueryable orderedQueryable)
            : base(orderedQueryable, jsonQLQueryable.JsonQLIncluder, jsonQLQueryable.JsonQLLinqer)
        {
            OrderedQueryable = orderedQueryable;
            JsonQLResource = jsonQLQueryable.JsonQLResource;
        }

        internal IOrderedQueryable OrderedQueryable { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public JsonQLQueryable_Ordered ThenBy(string selector)
        {
            OrderedQueryable = JsonQLLinqer.ThenBy(OrderedQueryable, selector, Parameters);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public JsonQLQueryable_Ordered ThenByDescending(string selector)
        {
            OrderedQueryable = JsonQLLinqer.ThenByDescending(OrderedQueryable, selector, Parameters);
            return this;
        }
    }
}
