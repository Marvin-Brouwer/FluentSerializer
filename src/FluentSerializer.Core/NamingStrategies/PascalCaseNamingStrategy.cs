using System.Reflection;

namespace FluentSerializer.Core.NamingStrategies
{
    public class PascalCaseNamingStrategy : CamelCaseNamingStrategy
    {
        // Just use the camelCase logic here
        public override string GetName(PropertyInfo property)
        {
            var camelCaseName = base.GetName(property);

            return $"{char.ToUpper(camelCaseName[0])}{camelCaseName[1..]}";
        }
    }
}
