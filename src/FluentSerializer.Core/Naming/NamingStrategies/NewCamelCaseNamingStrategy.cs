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
	private bool _loweredFirst = false;
	private int _skippedCount = 0;

	/// <remarks>
	/// Since we don't have whitespaces in C# property and class names we can just lowercase the first char.
	/// </remarks>
	protected override void ConvertCasing(in ReadOnlySpan<char> sourceSpan, ref Span<char> characterSpan)
	{
		for (var i = 0; i < sourceSpan.Length; i++)
		{
			var currentChar = sourceSpan[i];

			if (!_loweredFirst)
			{
				_loweredFirst = true;
				characterSpan[i - _skippedCount] = char.ToLowerInvariant(currentChar);
				continue;
			}
			if (currentChar == NamingConstants.ForbiddenCharacters.Underscore
			||  currentChar == NamingConstants.ForbiddenCharacters.Plus
			||  currentChar == NamingConstants.ForbiddenCharacters.Minus)
			{
				characterSpan[i - _skippedCount] = char.ToUpperInvariant(currentChar);
				_skippedCount++;
				continue;
			}
			characterSpan[i - _skippedCount] = currentChar;

			// Stop if we encounter a generic type indicator
			if (sourceSpan.Length > i + 1 && sourceSpan[i + 1] == NamingConstants.GenericTypeMarker)
			{
				_skippedCount += sourceSpan.Length - (i +1);
				break;
			}
		}

		characterSpan = characterSpan[..^_skippedCount];
		_loweredFirst = false;
		_skippedCount = 0;
	}
}