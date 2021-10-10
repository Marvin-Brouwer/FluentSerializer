namespace FluentSerializer.Core.NamingStrategies
{
    public class PascalCaseNamingStrategy : CamelCaseNamingStrategy
    {
        // Just use the camelCase logic here
        protected override string GetName(string name)
        {
            var camelCaseName = base.GetName(name);

            return $"{char.ToUpper(camelCaseName[0])}{camelCaseName[1..]}";
        }
    }
}
