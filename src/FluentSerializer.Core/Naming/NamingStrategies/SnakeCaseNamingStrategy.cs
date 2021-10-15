using System.Linq;

namespace FluentSerializer.Core.Naming.NamingStrategies
{
    public class SnakeCaseNamingStrategy : CamelCaseNamingStrategy
    {
        // Just use the camelCase logic here
        protected override string GetName(string name)
        {
            var camelCaseName = base.GetName(name);

            return string.Join(string.Empty, camelCaseName
                .AsEnumerable()
                .Select(character => char.IsUpper(character) 
                    ? $"_{char.ToLowerInvariant(character)}" 
                    : char.ToLowerInvariant(character).ToString()));
        }
    }
}