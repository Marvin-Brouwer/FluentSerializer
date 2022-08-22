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
	protected override ReadOnlySpan<char> GetName(string name)
	{
		Span<char> characterSpan = stackalloc char[name.Length *2];

		ConvertCasing(name, ref characterSpan);

		var newName = characterSpan.ToString();
		Guard.Against.InvalidName(newName);
		return newName;
	}

	/// <inheritdoc />
	protected override void ConvertCasing(in ReadOnlySpan<char> sourceSpan, ref Span<char> characterSpan)
	{
		var charCount = 0;

		for (var iteration = 0; iteration < sourceSpan.Length; iteration++)
		{
			var currentChar = sourceSpan[iteration];

			if (char.IsUpper(currentChar))
			{
				characterSpan[charCount] = NamingConstants.SpecialCharacters.Underscore;
				charCount.Increment();
				characterSpan[charCount] = char.ToLowerInvariant(currentChar);
				charCount.Increment();
				continue;
			}

			if (currentChar == NamingConstants.SpecialCharacters.Underscore
			 || currentChar == NamingConstants.SpecialCharacters.Plus
			 || currentChar == NamingConstants.SpecialCharacters.Minus)
			{
				characterSpan[charCount] = NamingConstants.SpecialCharacters.Underscore;
				charCount.Increment();
				continue;
			}

			characterSpan[charCount] = char.ToLowerInvariant(currentChar);

			// Stop if we encounter a generic type indicator
			if (currentChar == NamingConstants.GenericTypeMarker) break;

			if (sourceSpan.Length == iteration) break;
			charCount.Increment();
		}

		characterSpan = characterSpan[1..charCount];
	}
}