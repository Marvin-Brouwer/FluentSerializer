using FluentSerializer.Core.Context;
using System;
using System.Reflection;
using System.Text.Json;

namespace FluentSerializer.Core.NamingStrategies
{
    public class CamelCaseNamingStrategy : INamingStrategy
    {
        public virtual string GetName(PropertyInfo property, INamingContext _) => GetName(property.Name);
        public virtual string GetName(Type classType, INamingContext _) => GetName(classType.Name);
        protected virtual string GetName(string name) => JsonNamingPolicy.CamelCase.ConvertName(name.Split('`')[0]);
    }
}
