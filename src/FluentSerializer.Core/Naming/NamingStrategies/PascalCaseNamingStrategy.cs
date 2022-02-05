namespace FluentSerializer.Core.Naming.NamingStrategies;

/// <summary>
/// Convert class and property names to PascalCase <br />
/// <example>
/// SomeName => SomeName
/// </example>
/// </summary>
public class PascalCaseNamingStrategy : CamelCaseNamingStrategy
{
	/// <inheritdoc />
	protected override string GetName(string name)
	{
		// Just use the camelCase logic here
		var camelCaseName = base.GetName(name);

		return $"{char.ToUpper(camelCaseName[0])}{camelCaseName[1..]}";
	}
}