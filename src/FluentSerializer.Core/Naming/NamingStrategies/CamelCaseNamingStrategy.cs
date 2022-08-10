using FluentSerializer.Core.Constants;
using FluentSerializer.Core.Extensions;

using System;

namespace FluentSerializer.Core.Naming.NamingStrategies;

/// <summary>
/// Convert class and property names to camelCase <br />
/// <example>
/// SomeName => someName
/// </example>
/// </summary>
public sealed class CamelCaseNamingStrategy : AbstractSpanNamingStrategy
{
	/// <remarks>
	/// Since we don't have whitespaces in C# property and class names we can just lowercase the first char.
	/// </remarks>
	protected override void ConvertCasing(in ReadOnlySpan<char> sourceSpan, ref Span<char> characterSpan)
	{
		for (var iteration = 0; iteration < sourceSpan.Length; iteration++)
		{
			var currentChar = sourceSpan[iteration];

			if (CharCount == 0)
			{
				characterSpan[CharCount] = char.ToLowerInvariant(currentChar);
				CharCount.Increment();
				continue;
			}
			if (currentChar == NamingConstants.SpecialCharacters.Underscore
			||  currentChar == NamingConstants.SpecialCharacters.Plus
			||  currentChar == NamingConstants.SpecialCharacters.Minus)
			{
				if (sourceSpan.Length > iteration)
				{
					characterSpan[CharCount] = char.ToUpperInvariant(sourceSpan[iteration + 1]);
					CharCount.Increment();
				}
				iteration.Increment();
				continue;
			}
			characterSpan[CharCount] = currentChar;

			// Stop if we encounter a generic type indicator
			if (currentChar == NamingConstants.GenericTypeMarker) break;

			if (sourceSpan.Length == iteration) break;
			CharCount.Increment();
		}

		characterSpan = characterSpan[..CharCount];
	}
}