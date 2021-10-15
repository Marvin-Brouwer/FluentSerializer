using System;
using System.Reflection;
using FluentSerializer.Core.Context;

namespace FluentSerializer.Core.Naming.NamingStrategies
{
    public class LowerCaseNamingStrategy : INamingStrategy
    {
        public virtual string GetName(PropertyInfo property, INamingContext _) => property.Name.Split('`')[0].ToLowerInvariant();
        public virtual string GetName(Type name, INamingContext _) => name.Name.Split('`')[0].ToLowerInvariant();
    }
}
