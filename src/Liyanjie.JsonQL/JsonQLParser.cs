using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Liyanjie.JsonQL
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonQLParser
    {
        #region ParseAccess

        const string pattern = @"\([^\(\)]*\)";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public static string[] ParseAccess(string template)
        {
            var segments = new Dictionary<string, string>();
            while (Regex.IsMatch(template, pattern))
            {
                foreach (var item in Regex.Matches(template, pattern))
                {
                    var segment = item.ToString();
                    var key = $"`{segments.Count}";
                    template = template.Replace(segment, key);
                    segments.Add(key, segment);
                }
            }
            var result = template.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < result.Length; i++)
            {
                var segment = result[i];
                foreach (var key in segments.Keys.Reverse())
                {
                    segment = segment.Replace(key, segments[key]);
                }
                result[i] = segment;
            }

            return result;
        }

        #endregion

        #region ParseInclude

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="templates"></param>
        /// <returns></returns>
        public static string[] ParseInclude(string template, IDictionary<string, string> templates)
        {
            templates ??= new Dictionary<string, string>();
            var output = ParseInclude_(template, templates).Distinct().ToArray();
            return output;
        }
        static IList<string> ParseInclude_(string template, IDictionary<string, string> templates)
        {
            var paths = new List<string>();

            if (template.StartsWith("#"))
            {
                template = templates[template.TrimEnd(']').TrimEnd('[')];
                var properties = template.TrimStart('{').TrimEnd('}').Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var property in properties)
                {
                    if (property.IndexOf("$.") > -1)
                        paths.AddRange(ParseInclude_(property.Substring(property.IndexOf("$.")), templates));
                    else if (property.IndexOf('#') > -1)
                        paths.AddRange(ParseInclude_(property.Substring(property.IndexOf('#')), templates));
                }
            }
            else if (template.StartsWith("$."))
            {
                var @string = template.Substring(template.IndexOf("$."));
                if (@string.IndexOf("=>") > -1)
                    paths.Add(@string.Substring(0, @string.IndexOf("=>")).Substring(2));
                else if (@string.LastIndexOf('.') > 1)
                    paths.Add(@string.Substring(0, @string.LastIndexOf('.')).Substring(2));
            }

            return paths;
        }

        #endregion

        #region ParseMethod

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public static (string Name, string Parameter, IList<string> Variables) ParseMethod(string template)
        {
            if (!Regex.IsMatch(template, @"^[a-zA-Z_]\w*\("))
                throw new Exception("Parse method call error!");

            var name = template.Substring(0, template.IndexOf('('));
            var param = template.Substring(template.IndexOf('('));

            var matches = Regex.Matches(param, @"[\+\-\*\/\=\>\<\&\^\|\!\%\(]\$[a-zA-Z1-9_]+");
            var parameters = new List<string>();
            foreach (var item in matches)
            {
                var parameter = item.ToString().Substring(1);
                param = param.Replace(parameter, $"@{parameters.Count}");
                if (!parameters.Contains(parameter))
                    parameters.Add(parameter);
            }

            return (name, param.TrimStart('(').TrimEnd(')'), parameters);
        }

        #endregion

        #region ParseSelect

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="templates"></param>
        /// <returns></returns>
        public static string ParseSelect(string template, IDictionary<string, string> templates)
        {
            templates ??= new Dictionary<string, string>();
            var output = ParseObject(template, templates).ToString();
            return output;
        }

        static Object ParseObject(string template, IDictionary<string, string> templates)
        {
            var @object = new Object();

            if (template.StartsWith("#"))
            {
                template = templates[template];
                var properties = template.TrimStart('{').TrimEnd('}').Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var property in properties)
                {
                    if (property.Contains(':'))
                    {
                        var namedProperty = property.Split(':');
                        var tmp_property = namedProperty[1];
                        if (tmp_property.Contains("=>"))
                            tmp_property = tmp_property.Substring(0, property.IndexOf("=>"));

                        if (tmp_property.StartsWith("#"))
                            @object.AddProperty(ParseObject(namedProperty[1], templates).Properties.ToArray());
                        else if (tmp_property.StartsWith("`"))
                        {
                            //计算表达式不处理
                        }
                        else
                        {
                            var segment = tmp_property.StartsWith("$.") ? tmp_property.Substring(2) : tmp_property;
                            if (segment.IndexOf('.') > 0)
                                @object.AddProperty(ParseProperty(segment.Split('.')));
                            else
                                @object.AddProperty(new Property
                                {
                                    Name = segment,
                                });
                        }
                    }
                    else
                    {
                        @object.AddProperty(new Property
                        {
                            Name = property,
                        });
                    }
                }
            }
            else if (template.StartsWith("$."))
            {
                var segment = template.Substring(2);
                if (segment.IndexOf('.') > 0)
                    @object.AddProperty(ParseProperty(segment.Split('.')));
                else
                    @object.AddProperty(new Property
                    {
                        Name = segment,
                    });
            }

            return @object;
        }
        static Property ParseProperty(string[] segments, int index = 0)
        {
            if (index < segments.Length - 1)
                return new Property
                {
                    Name = segments[index],
                    Value = new Object().AddProperty(ParseProperty(segments, ++index)),
                };
            else
                return new Property
                {
                    Name = string.Join(".", segments)
                };
        }

        class Property
        {
            public string Name { get; set; }
            public Object Value { get; set; }
        }
        class Object
        {
            internal IList<Property> Properties { get; set; } = new List<Property>();

            public Object AddProperty(params Property[] properties)
            {
                foreach (var property in properties)
                {
                    var find = Properties.FirstOrDefault(_ => _.Name == property.Name);
                    if (find == null)
                        Properties.Add(property);
                    else if (property.Value != null)
                        (find.Value ??= new Object()).AddProperty(property.Value.Properties.ToArray());
                }
                return this;
            }

            public override string ToString()
            {
                if (Properties.Count > 0)
                    return $"new{{{string.Join(",", Properties.Select(_ => _.Value == null ? _.Name : $"{_.Name}={_.Value.ToString()}"))}}}";
                return null;
            }
        }

        #endregion

        #region ParserQuery

        const string regex_Resource = @"[a-zA-Z_]\w*\[\]";
        const string regex_Expression = @"{{[^{}]*}}";
        const string regex_Template = @"{[^{}]*}";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="resources"></param>
        /// <param name="expressions"></param>
        /// <param name="templates"></param>
        /// <param name="entry"></param>
        public static void ParseQuery(string query,
            out Dictionary<string, string> resources,
            out Dictionary<string, string> expressions,
            out Dictionary<string, string> templates,
            out string entry)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentNullException(nameof(query));

            resources = new Dictionary<string, string>();
            expressions = new Dictionary<string, string>();
            templates = new Dictionary<string, string>();

            var queue = new Queue<char>();
            var quota1 = false;
            var quota2 = false;
            foreach (var @char in query)
            {
                if (@char == '\'')
                {
                    queue.Enqueue(@char);
                    if (!quota2)
                        quota1 = !quota1;
                }
                if (@char == '"')
                {
                    queue.Enqueue(@char);
                    if (!quota1)
                        quota2 = !quota2;
                }
                else if (@char == '@' || @char == '#')
                {
                    if (quota1 || quota2)
                        queue.Enqueue(@char);
                    else
                        throw new Exception("Parse query error!");
                }
                else if (@char == 9 || @char == 10 || @char == 13 || @char == 32)
                {
                    if (quota1 || quota2)
                        queue.Enqueue(@char);
                }
                else if (@char < 32)
                {
                    if (quota1 || quota2)
                        queue.Enqueue(@char);
                    else
                        throw new Exception("Parse query error!");
                }
                else
                    queue.Enqueue(@char);
            }

            var @string = string.Join(string.Empty, queue.ToArray());
            {
                foreach (var item in Regex.Matches(@string, regex_Resource))
                {
                    var match = item as Match;
                    var value = match.Value;
                    if (!resources.ContainsValue(value))
                    {
                        var key = $"@{resources.Count}";
                        resources.Add(key, value);
                        @string = @string.Replace(value, key);
                    }
                }
            }
            {
                foreach (var item in Regex.Matches(@string, regex_Expression))
                {
                    var match = item as Match;
                    var value = match.Value;
                    if (!expressions.ContainsValue(value))
                    {
                        var key = $"`{expressions.Count}";
                        expressions.Add(key, value);
                        @string = @string.Replace(value, key);
                    }
                }
            }

            do
            {
                foreach (var item in Regex.Matches(@string, regex_Template))
                {
                    var match = item as Match;
                    var value = match.Value;
                    if (!templates.ContainsValue(value))
                    {
                        var key = $"#{templates.Count}";
                        templates.Add(key, value);
                        @string = @string.Replace(value, key);
                    }
                }
            } while (Regex.IsMatch(@string, regex_Template));

            entry = @string;
        }

        #endregion
    }
}
