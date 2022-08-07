using System;
using System.Reflection;
using Ardalis.GuardClauses;
using FluentSerializer.Core.Constants;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;

namespace FluentSerializer.Core.Naming.NamingStrategies;

/// <summary>
/// Convert class and property names to camelCase <br />
/// <example>
/// SomeName => someName
/// </example>
/// </summary>
public sealed class NewCamelCaseNamingStrategy : AbstractSpanNamingStrategy
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
			if (currentChar == NamingConstants.ForbiddenCharacters.Underscore
			||  currentChar == NamingConstants.ForbiddenCharacters.Plus
			||  currentChar == NamingConstants.ForbiddenCharacters.Minus)
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