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
public sealed class NewCamelCaseNamingStrategy : INamingStrategy
{
	/// <inheritdoc />
	public string GetName(in PropertyInfo property, in Type propertyType, in INamingContext namingContext) => GetName(property.Name);
	/// <inheritdoc />
	public string GetName(in Type classType, in INamingContext namingContext) => GetName(classType.Name);

	/// <summary>
	/// Convert a string value to camelCase
	/// </summary>
	private static string GetName(in string name)
	{
		Guard.Against.InvalidName(in name);

		var properClassName = name.Split('`')[0];

		return string.Create(properClassName.Length, name, ConvertCasing);
	}

	private static void ConvertCasing(Span<char> characterSpan, string originalString)
	{
		var sourceSpan = originalString.AsSpan();
		sourceSpan.CopyTo(characterSpan);
		ConvertCasing(characterSpan);
	}

	/// <remarks>
	/// Since we don't have whitespaces in C# property and class names we can just lowercase the first char.
	/// </remarks>
	private static void ConvertCasing(Span<char> characterSpan)
	{
		const char classSeparator = '`';

		for (var i = 0; i < characterSpan.Length; i++)
		{
			if (i == 0 && !char.IsUpper(characterSpan[i]))
				characterSpan[i] = char.ToLowerInvariant(characterSpan[i]);

			// Stop if we encounter a generic type indicator
			if (characterSpan.Length > i + 1 && characterSpan[i + 1] == classSeparator) break;
		}
	}
}