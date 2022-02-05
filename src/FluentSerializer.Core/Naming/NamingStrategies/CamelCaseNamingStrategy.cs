using System;
using System.Reflection;
using Ardalis.GuardClauses;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;

namespace FluentSerializer.Core.Naming.NamingStrategies;

/// <summary>
/// Convert class and property names to camelCase <br />
/// <example>
/// SomeName => someName
/// </example>
/// </summary>
public class CamelCaseNamingStrategy : INamingStrategy
{
	/// <inheritdoc />
	public virtual string GetName(PropertyInfo property, INamingContext namingContext) => GetName(property.Name);
	/// <inheritdoc />
	public virtual string GetName(Type classType, INamingContext namingContext) => GetName(classType.Name);

	/// <summary>
	/// Convert a string value to camelCase
	/// </summary>
	protected virtual string GetName(string name)
	{
		Guard.Against.InvalidName(name);

		var properClassName = name.Split('`')[0];

		return string.Create(properClassName.Length, properClassName, ConvertCasing);
	}

	private static void ConvertCasing(Span<char> characterSpan, string originalString)
	{
		var sourceSpan = originalString.AsSpan();
		sourceSpan.CopyTo(characterSpan);
		ConvertCasing(characterSpan);
	}

	/// <remarks>
	/// This is based on the <see><cref>System.Text.Json.JsonCamelCaseNamingPolicy</cref></see> but,
	/// since dotnet standard doesn't have that we needed our own version.
	/// </remarks>
	private static void ConvertCasing(Span<char> characterSpan)
	{
		const char spaceCharacter = ' ';

		for (var i = 0; i < characterSpan.Length; i++)
		{
			if (i == 1 && !char.IsUpper(characterSpan[i])) break;

			var hasNext = i + 1 < characterSpan.Length;

			// Stop when next char is already lowercase.
			if (i > 0 && hasNext && !char.IsUpper(characterSpan[i + 1]))
			{
				// If the next char is a space, lowercase current char before exiting.
				if (characterSpan[i + 1] == spaceCharacter)
				{
					characterSpan[i] = char.ToLowerInvariant(characterSpan[i]);
				}

				break;
			}

			characterSpan[i] = char.ToLowerInvariant(characterSpan[i]);
		}
	}
}