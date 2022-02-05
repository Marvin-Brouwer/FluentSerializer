using System.Linq;

namespace FluentSerializer.Core.Naming.NamingStrategies;

/// <summary>
/// Convert class and property names to snake_case <br />
/// <example>
/// SomeName => some_name
/// </example>
/// </summary>
public class SnakeCaseNamingStrategy : CamelCaseNamingStrategy
{
	/// <inheritdoc />
	protected override string GetName(string name)
	{
		// Just use the camelCase logic here
		var camelCaseName = base.GetName(name);

		return string.Join(string.Empty, camelCaseName
			.AsEnumerable()
			.Select(character => char.IsUpper(character) 
				? $"_{char.ToLowerInvariant(character)}" 
				: char.ToLowerInvariant(character).ToString()));
	}
}