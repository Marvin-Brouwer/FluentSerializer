using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace FluentSerializer.Core.Extensions;

/// <summary>
/// Extens to make checking characters and positioning in sring values slightly more readable
/// </summary>
public static class StringExtensions
{
	/// <summary>
	/// Check whether the current <paramref name="offset"/> fits within the <paramref name="text"/> size
	/// </summary>
#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public static bool WithinCapacity(in this ReadOnlySpan<char> text, in int offset) => offset < text.Length;

	/// <summary>
	/// Check if the <paramref name="text"/> contains this <paramref name="character"/> at the current <paramref name="offset"/>
	/// </summary>
#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public static bool HasCharacterAtOffset(in this ReadOnlySpan<char> text, in int offset, in char character) =>
		text[offset].Equals(character);

	/// <summary>
	/// Check if the <paramref name="text"/> contains these <paramref name="characters"/> at the current <paramref name="offset"/>
	/// </summary>
#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public static bool HasCharactersAtOffset(in this ReadOnlySpan<char> text, in int offset, in ReadOnlySpan<char> characters) =>
		text[offset..(offset + characters.Length)].Equals(characters, StringComparison.OrdinalIgnoreCase);

#if NETSTANDARD2_0
	/// <inheritdoc cref="HasCharactersAtOffset(in ReadOnlySpan{char}, in int, in ReadOnlySpan{char})"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining), ExcludeFromCodeCoverage]
	public static bool HasCharactersAtOffset(this ReadOnlySpan<char> text, in int offset, in string characters) =>
		HasCharactersAtOffset(text, in offset, characters.AsSpan());
#endif

	/// <summary>
	/// Check if the <paramref name="text"/> contains these characters at the current <paramref name="offset"/>
	/// </summary>
	/// <remarks>
	/// Special overload because XML requires 2 char tokens sometimes
	/// </remarks>
#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public static bool HasCharactersAtOffset(in this ReadOnlySpan<char> text, in int offset, in char character1, in char character2) =>
		HasCharacterAtOffset(in text, in offset, in character1) &&
		HasCharacterAtOffset(in text, offset + 1, in character2);

	/// <summary>
	/// Check if the <paramref name="text"/> contains whitespace at the current <paramref name="offset"/>
	/// </summary>
#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public static bool HasWhitespaceAtOffset(in this ReadOnlySpan<char> text, in int offset) =>
		char.IsWhiteSpace(text[offset]);
}