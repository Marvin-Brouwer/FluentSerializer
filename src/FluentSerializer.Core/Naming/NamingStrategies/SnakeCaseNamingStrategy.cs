using Ardalis.GuardClauses;

using FluentSerializer.Core.Constants;
using FluentSerializer.Core.Extensions;

using System;

namespace FluentSerializer.Core.Naming.NamingStrategies;

/// <summary>
/// Convert class and property names to snake_case <br />
/// <example>
/// SomeName => some_name
/// </example>
/// </summary>
public sealed class SnakeCaseNamingStrategy : AbstractSpanNamingStrategy
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
				characterSpan[CharCount] = NamingConstants.SpecialCharacters.Underscore;
				CharCount.Increment();
				characterSpan[CharCount] = char.ToLowerInvariant(currentChar);
				CharCount.Increment();
				continue;
			}

			if (currentChar == NamingConstants.SpecialCharacters.Underscore
			 || currentChar == NamingConstants.SpecialCharacters.Plus
			 || currentChar == NamingConstants.SpecialCharacters.Minus)
			{
				characterSpan[CharCount] = NamingConstants.SpecialCharacters.Underscore;
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