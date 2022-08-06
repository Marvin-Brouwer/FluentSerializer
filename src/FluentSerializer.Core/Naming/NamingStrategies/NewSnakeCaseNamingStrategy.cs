using System;
using System.Reflection;
using Ardalis.GuardClauses;
using FluentSerializer.Core.Constants;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;

namespace FluentSerializer.Core.Naming.NamingStrategies;

/// <summary>
/// Convert class and property names to snake_case <br />
/// <example>
/// SomeName => some_name
/// </example>
/// </summary>
public sealed class NewSnakeCaseNamingStrategy : AbstractSpanNamingStrategy
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
		for (var i = 0; i < sourceSpan.Length; i++)
		{
			var currentChar = sourceSpan[i];

			if (char.IsUpper(currentChar))
			{
				characterSpan[CharCount] = NamingConstants.ForbiddenCharacters.Underscore;
				CharCount++;
				characterSpan[CharCount] = char.ToLowerInvariant(currentChar);
				CharCount++;
				continue;
			}

			if (currentChar == NamingConstants.ForbiddenCharacters.Underscore
			 || currentChar == NamingConstants.ForbiddenCharacters.Plus
			 || currentChar == NamingConstants.ForbiddenCharacters.Minus)
			{
				characterSpan[CharCount] = NamingConstants.ForbiddenCharacters.Underscore;
				CharCount++;
				continue;
			}

			characterSpan[CharCount] = char.ToLowerInvariant(currentChar);
			CharCount++;
			if (i == 0)
			{
				continue;
			}

			// Stop if we encounter a generic type indicator
			if (sourceSpan.Length > i + 1 && sourceSpan[i + 1] == NamingConstants.GenericTypeMarker) {
				break;
			}
		}

		characterSpan = characterSpan[1..CharCount];
	}
}