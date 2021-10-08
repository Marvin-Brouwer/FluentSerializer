using System.Reflection;
using System.Text.Json;

namespace FluentSerializer.Xml.Profiles
{
    public class LowerCaseNamingStrategy : INamingStrategy
    {
        public virtual string GetName(PropertyInfo property) => property.Name.ToLowerInvariant();
    }
}
