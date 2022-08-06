using System;
using System.Reflection;
using Ardalis.GuardClauses;
using FluentSerializer.Core.Constants;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;

namespace FluentSerializer.Core.Naming.NamingStrategies;

/// <summary>
/// Convert class and property names to PascalCase <br />
/// <example>
/// SomeName => SomeName
/// </example>
/// </summary>
public sealed class NewPascalCaseNamingStrategy : AbstractSpanNamingStrategy
{
	private bool _upperedFirst = false;
	private int _skippedCount = 0;

	/// <remarks>
	/// Since we don't have whitespaces in C# property and class names we can just uppercase the first char.
	/// </remarks>
	protected override void ConvertCasing(in ReadOnlySpan<char> sourceSpan, ref Span<char> characterSpan)
	{
		for (var i = 0; i < sourceSpan.Length; i++)
		{
			var currentChar = sourceSpan[i];

			if (!_upperedFirst)
			{
				_upperedFirst = true;
				characterSpan[i - _skippedCount] = char.ToUpperInvariant(currentChar);
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
				_skippedCount += sourceSpan.Length - (i + 1);
				break;
			}
		}

		characterSpan = characterSpan[..^_skippedCount];
		_upperedFirst = false;
		_skippedCount = 0;
	}
}