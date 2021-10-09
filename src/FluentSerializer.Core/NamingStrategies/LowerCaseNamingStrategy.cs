using System.Reflection;

namespace FluentSerializer.Core.NamingStrategies
{
    public class LowerCaseNamingStrategy : INamingStrategy
    {
        public virtual string GetName(PropertyInfo property) => property.Name.ToLowerInvariant();
    }
}
