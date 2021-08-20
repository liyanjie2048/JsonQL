using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Liyanjie.JsonQL.Internal;
using Liyanjie.TypeBuilder;

namespace Liyanjie.JsonQL
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class JsonQLHandler
    {
        readonly JsonQLOptions options;
        readonly JsonQLResourceTable jsonQLResourceTable;

        IJsonQLRequest jsonQLRequest;
        Dictionary<string, JsonQLResource> resources;
        Dictionary<string, string> expressions;
        Dictionary<string, string> templates;
        string entry;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonQLResourceTable"></param>
        /// <param name="options"></param>
        public JsonQLHandler(
            JsonQLOptions options,
            JsonQLResourceTable jsonQLResourceTable)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.jsonQLResourceTable= jsonQLResourceTable?? throw new ArgumentNullException(nameof(jsonQLResourceTable));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonQLRequest"></param>
        /// <returns></returns>
        public async Task<string> HandleAsync(IJsonQLRequest jsonQLRequest)
        {
            this.jsonQLRequest = jsonQLRequest;

            JsonQLParser.ParseQuery(await jsonQLRequest.GetQueryAsync(), out var _resources, out var _expressions, out var _templates, out var _entry);

            resources = _resources.ToDictionary(_ => _.Key, _ => jsonQLResourceTable.GetResource(_.Value));
            expressions = _expressions;
            templates = _templates;
            entry = _entry;

            var @object = await CreateObjectAsync(templates[entry]);

            return JsonSerializer.Serialize(@object);
        }

        /// <summary>
        /// 从类json模板创建对象
        /// </summary>
        /// <param name="template">“{}”包围的类json对象模板</param>
        /// <param name="variables">要从中取值的变量</param>
        /// <param name="object">要从中取值的对象</param>
        /// <returns></returns>
        async Task<object> CreateObjectAsync(
            string template,
            IDictionary<string, object> variables = null,
            object @object = null)
        {
            var properties = template.TrimStart('{').TrimEnd('}').Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            var dic = new Dictionary<string, object>();
            var variables_temp = variables.Clone();
            foreach (var property in properties)
            {
                var segments = property.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                var name = segments[0];
                var valueTemplate = segments.Length > 1 ? segments[1] : $"$.{name}";
                if (name.StartsWith("$"))//是变量
                    variables_temp[name] = await GetValue1Async(valueTemplate, variables_temp, true, @object);
                else
                    dic[name] = await GetValue1Async(valueTemplate, variables_temp, false, @object);
            }

            return TypeFactory.CreateObject(dic);
        }

        async Task<object> GetValue1Async(
            string template,
            IDictionary<string, object> variables,
            bool isVariable,
            object @object = null)
        {
            static (bool IsArray, int Count) isArray(ref string inputTemplate)
            {
                var pattern = @"\[(?<count>\d*)?\]$";
                var match = Regex.Match(inputTemplate, pattern);
                if (match.Success)
                {
                    inputTemplate = Regex.Replace(inputTemplate, pattern, string.Empty);
                    return (true, int.TryParse(match.Groups["count"].Value, out var count) ? count : 0);
                }
                else
                    return (false, 0);
            }

            if (template.IndexOf("=>") > 0) //枚举操作
            {
                var segments = template.Split(new[] { "=>" }, 2, StringSplitOptions.RemoveEmptyEntries);

                var template_resource = segments[0];
                var template_enumeration = segments[1];

                var includes = JsonQLParser.ParseInclude(template_enumeration, templates);

                var (resource, accessor) = await ProcessResourceAsync(template_resource, includes, variables);
                var queryable = accessor is JsonQLQueryable jsonQLQueryable
                    ? jsonQLQueryable?.Queryable
                    : (accessor as IEnumerable)?.AsQueryable();
                if (queryable == null)
                    return null;

                if (options.JsonQLLinqer != null)
                {
                    var selector = JsonQLParser.ParseSelect(template_enumeration, templates);
                    if (!string.IsNullOrWhiteSpace(selector))
                        queryable = options.JsonQLLinqer.Select(queryable, selector);
                }

                var output = isArray(ref template_enumeration);
                if (output.IsArray)
                {
                    var list = new List<object>();
                    var source = options.JsonQLLinqer == null
                        ? QueryableHelper.ToList(queryable)
                        : options.JsonQLLinqer.ToList(queryable);
                    if (output.Count > 0)
                        source = source.Take(output.Count).ToList();
                    foreach (var item in source)
                    {
                        list.Add(await GetValue1Async(template_enumeration, variables, false, item));
                    }
                    return list;
                }
                else
                {
                    var first = options.JsonQLLinqer == null
                        ? QueryableHelper.FirstOrDefault(queryable)
                        : options.JsonQLLinqer.FirstOrDefault(queryable);
                    if (first == null)
                        return null;

                    return await GetValue1Async(template_enumeration, variables, false, first);
                }
            }
            else //取值操作
            {
                var value = await GetValue2Async(template, null, variables, @object);
                if (value is JsonQLQueryable)
                {
                    if (isVariable)
                        return value;
                    else
                    {
                        var queryable = (value as JsonQLQueryable).Queryable;
                        return options.JsonQLLinqer == null
                            ? QueryableHelper.ToList(queryable)
                            : options.JsonQLLinqer.ToList(queryable);
                    }
                }
                else
                    return value;
            }
        }

        async Task<object> GetValue2Async(
            string template,
            string[] includes,
            IDictionary<string, object> variables,
            object @object = null)
        {
            if ("$".Equals(template))//对象自身
                return @object;
            else if (template.StartsWith("$."))//对象的属性或方法
                return @object.GetValue(template.Substring(2));
            else if (template.StartsWith("`"))//表达式
                return EvaluateExpression(expressions[template], ref variables);
            else if (template.StartsWith("#"))//模板
                return await CreateObjectAsync(templates[template], variables, @object);
            else if (template.StartsWith("@"))//资源
                return await ProcessResourceAsync(template, includes, variables);
            else if (template.StartsWith("$"))//变量
                return await ProcessResourceAsync(template, includes, variables);
            else
                return JsonSerializer.Deserialize(template, typeof(object));
        }

        async Task<(JsonQLResource Resource, object ResourceAccessor)> ProcessResourceAsync(
            string template,
            string[] includes,
            IDictionary<string, object> variables)
        {
            await Task.FromResult(0);

            JsonQLResource resource = null;
            object resourceAccessor = null;

            var segments = JsonQLParser.ParseAccess(template);

            var segment0 = segments[0];
            if (segment0.StartsWith("@"))
            {
                resource = resources[segment0];
                resourceAccessor = new JsonQLQueryable(resource.BuildQuery(jsonQLRequest), options.JsonQLIncluder, options.JsonQLLinqer)
                {
                    JsonQLResource = resource
                }.Include(includes);
            }
            else if (segment0.StartsWith("$"))
                resourceAccessor = variables[segment0];

            if (resourceAccessor != null)
                for (var i = 1; i < segments.Length; i++)
                {
                    var segment = segments[i];
                    if (resourceAccessor is JsonQLQueryable jsonQLQueryable)
                    {
                        var (methodName, methodParameter, _variables) = JsonQLParser.ParseMethod(segment);
                        var parameters = _variables.ToDictionary(_ => _, _ => variables[_]);
                        var queryable = jsonQLQueryable.SetParameters(parameters);
                        switch (methodName.ToLower())
                        {
                            case "all":
                                resourceAccessor = queryable.All(methodParameter);
                                break;
                            case "any":
                                resourceAccessor = string.IsNullOrEmpty(methodParameter)
                                    ? queryable.Any()
                                    : queryable.Any(methodParameter);
                                break;
                            case "average":
                                resourceAccessor = string.IsNullOrEmpty(methodParameter)
                                    ? queryable.Average()
                                    : queryable.Average(methodParameter);
                                break;
                            case "count":
                                resourceAccessor = string.IsNullOrEmpty(methodParameter)
                                    ? queryable.Count()
                                    : queryable.Count(methodParameter);
                                break;
                            case "distinct":
                                resourceAccessor = queryable.Distinct();
                                break;
                            case "groupby":
                                resourceAccessor = queryable.GroupBy(methodParameter);
                                break;
                            case "max":
                                resourceAccessor = string.IsNullOrEmpty(methodParameter)
                                    ? queryable.Max()
                                    : queryable.Max(methodParameter);
                                break;
                            case "min":
                                resourceAccessor = string.IsNullOrEmpty(methodParameter)
                                    ? queryable.Min()
                                    : queryable.Min(methodParameter);
                                break;
                            case "orderby":
                                resourceAccessor = queryable.OrderBy(methodParameter);
                                break;
                            case "orderbydescending":
                                resourceAccessor = queryable.OrderByDescending(methodParameter);
                                break;
                            case "select":
                                resourceAccessor = queryable.Select(methodParameter);
                                break;
                            case "skip":
                                resourceAccessor = queryable.Skip(int.Parse(methodParameter));
                                break;
                            case "sum":
                                resourceAccessor = string.IsNullOrEmpty(methodParameter)
                                    ? queryable.Sum()
                                    : queryable.Sum(methodParameter);
                                break;
                            case "take":
                                resourceAccessor = queryable.Take(int.Parse(methodParameter));
                                break;
                            case "thenby":
                                resourceAccessor = (queryable as JsonQLQueryable_Ordered).ThenBy(methodParameter);
                                break;
                            case "thenbydescending":
                                resourceAccessor = (queryable as JsonQLQueryable_Ordered).ThenByDescending(methodParameter);
                                break;
                            case "where":
                                resourceAccessor = queryable.Where(methodParameter);
                                break;
                            default:
                                break;
                        }
                    }
                    else
                        throw new Exception($"无法对非资源对象调用 {segment} 方法");
                }

            return (resource, resourceAccessor);
        }

        object EvaluateExpression(string expression, ref IDictionary<string, object> variables)
        {
            if (options.JsonQLEvaluator == null)
                return "未提供JsonQLEvaluator实例";
            return options.JsonQLEvaluator.Evaluate(expression.TrimStart('{').TrimEnd('}'), ref variables);
        }
    }
}
