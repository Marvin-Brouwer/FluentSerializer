using FluentSerializer.Core.NamingStrategies;
using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace FluentSerializer.Xml.NamingStrategies
{
    internal class VerbatimGenericNamingStrategy : INamingStrategy
    {
        private static Regex GenericMatcher = new Regex("`[0-9]+",  RegexOptions.ECMAScript | RegexOptions.CultureInvariant);
        public string GetName(PropertyInfo property)
        {
            return GetName(property.PropertyType);
        }

        public string GetName(Type classType)
        {
            var containerTypeName = classType.Name.Replace("`1", "Of");
            containerTypeName = GenericMatcher.Replace(containerTypeName, "Containing");

            var typeInformation = classType.GetTypeInfo();
            if (!typeInformation.IsGenericType) return containerTypeName;
            var genericArguments = typeInformation.GenericTypeArguments;
            if (genericArguments is null) return containerTypeName;

            return containerTypeName + String.Join("+", genericArguments.Select(GetName));
        }
    }
}