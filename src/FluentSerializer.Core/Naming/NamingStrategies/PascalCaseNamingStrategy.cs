using FluentSerializer.Core.Constants;
using FluentSerializer.Core.Extensions;

using System;

namespace FluentSerializer.Core.Naming.NamingStrategies;

/// <summary>
/// Convert class and property names to PascalCase <br />
/// <example>
/// SomeName => SomeName
/// </example>
/// </summary>
public sealed class PascalCaseNamingStrategy : AbstractSpanNamingStrategy
{
	/// <remarks>
	/// Since we don't have whitespaces in C# property and class names we can just uppercase the first char.
	/// </remarks>
	protected override void ConvertCasing(in ReadOnlySpan<char> sourceSpan, ref Span<char> characterSpan)
	{
		var charCount = 0;

		for (var iteration = 0; iteration < sourceSpan.Length; iteration++)
		{
			var currentChar = sourceSpan[iteration];

			if (charCount == 0)
			{
				characterSpan[charCount] = char.ToUpperInvariant(currentChar);
				charCount.Increment();
				continue;
			}
			if (currentChar == NamingConstants.SpecialCharacters.Underscore
			|| currentChar == NamingConstants.SpecialCharacters.Plus
			|| currentChar == NamingConstants.SpecialCharacters.Minus)
			{
				if (sourceSpan.Length > iteration)
				{
					characterSpan[charCount] = char.ToUpperInvariant(sourceSpan[iteration + 1]);
					charCount.Increment();
				}
				iteration.Increment();
				continue;
			}
			characterSpan[charCount] = currentChar;

			// Stop if we encounter a generic type indicator
			if (currentChar == NamingConstants.GenericTypeMarker) break;

			if (sourceSpan.Length == iteration) break;
			charCount.Increment();
		}

		characterSpan = characterSpan[..charCount];
	}
}