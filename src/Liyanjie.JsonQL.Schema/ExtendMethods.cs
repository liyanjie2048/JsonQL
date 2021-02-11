using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Liyanjie.JsonQL.Schema
{
    internal static class ExtendMethods
    {
        public static IEnumerable<JsonQLSchemaProperty> GetSchemaProperties(this Type type)
            => type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .OrderBy(_ => _.Name)
                .Select(_ => new JsonQLSchemaProperty
                {
                    Name = _.Name.ToCamelCase(),
                    Type = _.PropertyType.GetSchemaType(),
                })
                .ToList();

        public static IEnumerable<JsonQLSchemaMethod> GetSchemaMethods(this Type type)
            => type
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .OrderBy(_ => _.Name)
                .Select(_ => new JsonQLSchemaMethod
                {
                    Name = _.Name.ToCamelCase(),
                    ReturnType = _.ReturnType.GetSchemaType(),
                    Parameters = _.GetSchemaParameters(),
                })
                .ToList();

        static List<JsonQLSchemaParameter> GetSchemaParameters(this MethodInfo method)
            => method.GetParameters()
                .Select(_ => new JsonQLSchemaParameter
                {
                    Name = _.Name.ToCamelCase(),
                    Type = _.ParameterType.GetSchemaType(),
                    IsOptional = _.IsOptional,
                })
                .ToList();

        public static JsonQLSchemaType GetSchemaType(this Type type)
        {
            var array = string.Empty;
            if (type.IsArray)
            {
                array = "[]";
                type = type.GetElementType();
            }
            var @return = new JsonQLSchemaType();
            if (type == typeof(JsonQL.JsonQLQueryable))
                @return.Name = nameof(JsonQL.JsonQLQueryable);
            else if ("Nullable`1".Equals(type.Name))
            {
                var (name, isBaseType) = type.GenericTypeArguments[0].Name.GetSchemaTypeName();
                @return.Name = $"{name}?{array}";
                @return.IsBaseType = isBaseType;
            }
            else if ("IList`1".Equals(type.Name) || "ICollection`1".Equals(type.Name) || "IEnumerable`1".Equals(type.Name))
            {
                var (name, isBaseType) = type.GenericTypeArguments[0].Name.GetSchemaTypeName();
                @return.Name = $"{name}[]{array}";
                @return.IsBaseType = isBaseType;
            }
            else
            {
                var (name, isBaseType) = type.Name.GetSchemaTypeName();
                @return.Name = $"{name}{array}";
                @return.IsBaseType = isBaseType;
            }
            return @return;
        }

        public static (string Name, bool IsValue) GetSchemaTypeName(this string typeName)
        {
            return typeName.ToLower() switch
            {
                "int16" => ("short", true),
                "uint16" => ("ushort", true),
                "int32" => ("int", true),
                "uint32" => ("uint", true),
                "int64" => ("long", true),
                "uint64" => ("ulong", true),
                "string" => (typeName.ToLower(), true),
                "boolean" => (typeName.ToLower(), true),
                "double" => (typeName.ToLower(), true),
                "decimal" => (typeName.ToLower(), true),
                "float" => (typeName.ToLower(), true),
                "byte" => (typeName.ToLower(), true),
                "sbyte" => (typeName.ToLower(), true),
                "object" => (typeName.ToLower(), true),
                _ => (typeName, false),
            };
        }

        public static string ToCamelCase(this string input)
        {
            if (input == null)
                return null;

            if (input.Length < 2)
                return input.ToLower();

            var i = 0;
            var find = false;

            for (; i < input.Length; i++)
            {
                var value = (int)input[i];
                if (value < 65 || value > 90)
                {
                    find = true;
                    break;
                }
            }

            if (!find)
                i++;

            if (i < 1)
                return input;
            else if (i == 1)
                return $"{input.Substring(0, 1).ToLower()}{input.Substring(1)}";
            else
                return $"{input.Substring(0, i - 1).ToLower()}{input.Substring(i - 1)}";
        }
    }
}
