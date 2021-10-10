using System;
using System.Reflection;
using System.Text.Json;

namespace FluentSerializer.Core.NamingStrategies
{
    public class CamelCaseNamingStrategy : INamingStrategy
    {
        public virtual string GetName(PropertyInfo property) => GetName(property.Name);
        public virtual string GetName(Type classType) => GetName(classType.Name);
        protected virtual string GetName(string name) => JsonNamingPolicy.CamelCase.ConvertName(name.Split('`')[0]);
    }
}
