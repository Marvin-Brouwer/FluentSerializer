using FluentSerializer.Core.Constants;
using FluentSerializer.Core.Extensions;

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FluentSerializer.Core.Naming.NamingStrategies;

/// <summary>
/// Convert class and property names to PascalCase <br />
/// <example>
/// SomeName => SomeName
/// </example>
/// </summary>
public sealed class PascalCaseNamingStrategy : AbstractSpanNamingStrategy
{
	/// <inheritdoc/>
	protected override void ConvertCasing(in ReadOnlySpan<char> sourceSpan, ref Span<char> characterSpan)
	{
		var charCount = 0;

		ref var sourceSearchSpace = ref MemoryMarshal.GetReference(sourceSpan);
		ref var targetSearchSpace = ref MemoryMarshal.GetReference(characterSpan);

		for (var iteration = 0; iteration < sourceSpan.Length; iteration.Increment())
		{
			ref var currentChar = ref Unsafe.Add(ref sourceSearchSpace, iteration);
			ref var targetChar = ref Unsafe.Add(ref targetSearchSpace, charCount);

			if (charCount == 0)
			{
				targetChar = char.ToUpperInvariant(currentChar);
				charCount.Increment();
				continue;
			}
			if (currentChar == NamingConstants.SpecialCharacters.Underscore
			|| currentChar == NamingConstants.SpecialCharacters.Plus
			|| currentChar == NamingConstants.SpecialCharacters.Minus)
			{
				if (sourceSpan.Length > iteration)
				{
					targetChar = char.ToUpperInvariant(sourceSpan[iteration + 1]);
					charCount.Increment();
				}
				iteration.Increment();
				continue;
			}
			targetChar = currentChar;

			// Stop if we encounter a generic type indicator
			if (currentChar == NamingConstants.GenericTypeMarker) break;

			if (sourceSpan.Length == iteration) break;
			charCount.Increment();
		}

		characterSpan = characterSpan[..charCount];
	}
}