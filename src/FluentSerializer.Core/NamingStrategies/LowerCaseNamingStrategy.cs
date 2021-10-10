using System;
using System.Reflection;

namespace FluentSerializer.Core.NamingStrategies
{
    public class LowerCaseNamingStrategy : INamingStrategy
    {
        public virtual string GetName(PropertyInfo property) => property.Name.Split('`')[0].ToLowerInvariant();
        public virtual string GetName(Type name) => name.Name.Split('`')[0].ToLowerInvariant();
    }
}
