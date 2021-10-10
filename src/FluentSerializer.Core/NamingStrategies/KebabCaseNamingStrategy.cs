using System.Linq;

namespace FluentSerializer.Core.NamingStrategies
{
    public class KebabCaseNamingStrategy : CamelCaseNamingStrategy
    {
        // Just use the camelCase logic here
        protected override string GetName(string name)
        {
            var camelCaseName = base.GetName(name);

            return string.Join(string.Empty, camelCaseName
                .AsEnumerable()
                .Select(character => char.IsUpper(character) ? $"-{character}" : character.ToString()));
        }
    }
}