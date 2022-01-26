using System;
using System.Runtime.CompilerServices;

namespace FluentSerializer.Core.Text.Extensions
{
	public static class StringExtensions
	{
#if NET6_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool WithinCapacity(in this ReadOnlySpan<char> text, in int offset) => offset < text.Length;

#if NET6_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool HasCharacterAtOffset(in this ReadOnlySpan<char> text, in int offset, in char character) =>
			text[offset].Equals(character);
#if NET6_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool HasCharactersAtOffset(in this ReadOnlySpan<char> text, in int offset, in ReadOnlySpan<char> characters) =>
			text[offset..(offset + characters.Length)].Equals(characters, StringComparison.OrdinalIgnoreCase);
#if NET6_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool HasCharactersAtOffset(in this ReadOnlySpan<char> text, in int offset, in char character1, in char character2) =>
			HasCharacterAtOffset(in text, in offset, in character1) &&
			HasCharacterAtOffset(in text, offset +1, in character2);
#if NET6_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool HasWhitespaceAtOffset(in this ReadOnlySpan<char> text, in int offset) =>
			char.IsWhiteSpace(text[offset]);
	}
}
