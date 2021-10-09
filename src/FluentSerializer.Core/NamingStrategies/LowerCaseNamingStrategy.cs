using System;
using System.Reflection;

namespace FluentSerializer.Core.NamingStrategies
{
    public class LowerCaseNamingStrategy : INamingStrategy
    {
        public virtual string GetName(PropertyInfo property) => property.Name.ToLowerInvariant();

        public string GetName(Type classType) => classType.Name.ToLowerInvariant();
    }
}
