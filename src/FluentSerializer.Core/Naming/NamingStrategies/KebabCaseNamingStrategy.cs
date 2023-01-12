using Ardalis.GuardClauses;

using FluentSerializer.Core.Constants;
using FluentSerializer.Core.Extensions;

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FluentSerializer.Core.Naming.NamingStrategies;

/// <summary>
/// Convert class and property names to kebab-case <br />
/// <example>
/// SomeName => some-name
/// </example>
/// </summary>
public sealed class KebabCaseNamingStrategy : AbstractSpanNamingStrategy
{
	/// <inheritdoc />
	protected override ReadOnlySpan<char> GetName(string name)
	{
		Span<char> characterSpan = stackalloc char[name.Length * 2];

#if NETSTANDARD2_0
		ConvertCasing(name.AsSpan(), ref characterSpan);
#else
		ConvertCasing(name, ref characterSpan);
#endif

		var newName = characterSpan.ToString();
		Guard.Against.InvalidName(newName);

#if NETSTANDARD2_0
		return newName.AsSpan();
#else
		return newName;
#endif
	}

	/// <inheritdoc />
	protected override void ConvertCasing(in ReadOnlySpan<char> sourceSpan, ref Span<char> characterSpan)
	{
		var charCount = 0;

		ref var sourceSearchSpace = ref MemoryMarshal.GetReference(sourceSpan);
		ref var targetSearchSpace = ref MemoryMarshal.GetReference(characterSpan);

		for (var iteration = 0; iteration < sourceSpan.Length; iteration.Increment())
		{
			ref var currentChar = ref Unsafe.Add(ref sourceSearchSpace, iteration);
			ref var targetChar = ref Unsafe.Add(ref targetSearchSpace, charCount);

			if (char.IsUpper(currentChar))
			{
				targetChar = NamingConstants.SpecialCharacters.Minus;
				charCount.Increment();
				targetChar = ref Unsafe.Add(ref targetSearchSpace, charCount);
				targetChar = char.ToLowerInvariant(currentChar);
				charCount.Increment();
				continue;
			}

			if (currentChar == NamingConstants.SpecialCharacters.Underscore
			 || currentChar == NamingConstants.SpecialCharacters.Plus
			 || currentChar == NamingConstants.SpecialCharacters.Minus)
			{
				targetChar = NamingConstants.SpecialCharacters.Minus;
				charCount.Increment();
				continue;
			}

			targetChar = char.ToLowerInvariant(currentChar);

			// Stop if we encounter a generic type indicator
			if (currentChar == NamingConstants.GenericTypeMarker) break;

			if (sourceSpan.Length == iteration) break;
			charCount.Increment();
		}

		characterSpan = characterSpan[1..charCount];
	}
}