using System.Linq;
using System.Reflection;

namespace FluentSerializer.Xml.Profiles
{
    public class KebabCaseNamingStrategy : CamelCaseNamingStrategy
    {
        // Just use the camelCase logic here
        public override string GetName(PropertyInfo property)
        {
            var camelCaseName = base.GetName(property);

            return string.Join(char.ConvertFromUtf32(0), camelCaseName
                .AsEnumerable()
                .Select(character => char.IsUpper(character) ? $"-{character}" : character.ToString()));
        }
    }
}