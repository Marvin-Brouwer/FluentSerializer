using System.Reflection;
using System.Text.Json;

namespace FluentSerializer.Core.NamingStrategies
{
    public class CamelCaseNamingStrategy : INamingStrategy
    {
        public virtual string GetName(PropertyInfo property) => JsonNamingPolicy.CamelCase.ConvertName(property.Name);
    }
}
