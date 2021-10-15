using System;
using System.Reflection;
using System.Text.Json;
using FluentSerializer.Core.Context;

namespace FluentSerializer.Core.Naming.NamingStrategies
{
    public class CamelCaseNamingStrategy : INamingStrategy
    {
        public virtual string GetName(PropertyInfo property, INamingContext namingContext) => GetName(property.Name);
        public virtual string GetName(Type classType, INamingContext namingContext) => GetName(classType.Name);
        protected virtual string GetName(string name) => JsonNamingPolicy.CamelCase.ConvertName(name.Split('`')[0]);
    }
}
