using System;
using System.Reflection;
using Ardalis.GuardClauses;
using FluentSerializer.Core.Constants;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;

namespace FluentSerializer.Core.Naming.NamingStrategies;

/// <summary>
/// Convert class and property names to kebab-case <br />
/// <example>
/// SomeName => some-name
/// </example>
/// </summary>
public sealed class NewKebabCaseNamingStrategy : AbstractSpanNamingStrategy
{
	/// <inheritdoc />
	protected override ReadOnlySpan<char> GetName(in string name)
	{
		Span<char> characterSpan = stackalloc char[name.Length *2];

		ConvertCasing(name, ref characterSpan);
		CharCount = 0;

		var newName = characterSpan.ToString();
		Guard.Against.InvalidName(newName);
		return newName;
	}

	/// <inheritdoc />
	protected override void ConvertCasing(in ReadOnlySpan<char> sourceSpan, ref Span<char> characterSpan)
	{
		for (var iteration = 0; iteration < sourceSpan.Length; iteration++)
		{
			var currentChar = sourceSpan[iteration];

			if (char.IsUpper(currentChar))
			{
				characterSpan[CharCount] = NamingConstants.ForbiddenCharacters.Minus;
				CharCount.Increment();
				characterSpan[CharCount] = char.ToLowerInvariant(currentChar);
				CharCount.Increment();
				continue;
			}

			if (currentChar == NamingConstants.ForbiddenCharacters.Underscore
			 || currentChar == NamingConstants.ForbiddenCharacters.Plus
			 || currentChar == NamingConstants.ForbiddenCharacters.Minus)
			{
				characterSpan[CharCount] = NamingConstants.ForbiddenCharacters.Minus;
				CharCount.Increment();
				continue;
			}

			characterSpan[CharCount] = char.ToLowerInvariant(currentChar);

			// Stop if we encounter a generic type indicator
			if (currentChar == NamingConstants.GenericTypeMarker) break;

			if (sourceSpan.Length == iteration) break;
			CharCount.Increment();
		}

		characterSpan = characterSpan[1..CharCount];
	}
}