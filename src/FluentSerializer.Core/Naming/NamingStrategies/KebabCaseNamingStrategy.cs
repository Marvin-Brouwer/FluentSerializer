using System.Linq;

namespace FluentSerializer.Core.Naming.NamingStrategies;

/// <summary>
/// Convert class and property names to kebab-case <br />
/// <example>
/// SomeName => some-name
/// </example>
/// </summary>
public class KebabCaseNamingStrategy : CamelCaseNamingStrategy
{
	/// <inheritdoc />
	protected override string GetName(string name)
	{
		// Just use the camelCase logic here
		var camelCaseName = base.GetName(name);

		return string.Join(string.Empty, camelCaseName
			.AsEnumerable()
			.Select(character => char.IsUpper(character) ? $"-{character}" : character.ToString()));
	}
}