using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace FluentSerializer.Core.Extensions;

/// <summary>
/// Extensions to make adjusting integer offsets slightly more readable
/// </summary>
public static class OffsetExtensions
{
	/// <summary>
	/// Adjust the offset by the length of the provided token
	/// </summary>
#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public static void AdjustForToken(ref this int offset, in ReadOnlySpan<char> token) => offset.Increment(token.Length);

#if NETSTANDARD2_0
	/// <inheritdoc cref="AdjustForToken(ref int, in ReadOnlySpan{char})"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining), ExcludeFromCodeCoverage]
	public static void AdjustForToken(ref this int offset, in string token) => AdjustForToken(ref offset, token.AsSpan());
#endif

	/// <summary>
	/// Adjust the offset by the length of the provided token
	/// </summary>
	/// <remarks>
	/// This overload is mainly for readability since the length of a single char is always 1
	/// </remarks>
#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public static void AdjustForToken(ref this int offset, in char token)
	{
		_ = token;
		offset.Increment();
	}

	/// <summary>
	/// Adjust the offset by the amount of whitespace found after the current <paramref name="offset"/>
	/// </summary>
#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public static void AdjustForWhiteSpace(ref this int offset, in ReadOnlySpan<char> text)
	{
		while (text.WithinCapacity(in offset) && text.HasWhitespaceAtOffset(in offset))
		{
			offset.Increment();
		}
	}

	/// <summary>
	/// Adjust the offset by one
	/// </summary>
#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public static void Increment(ref this int offset) => offset++;

	/// <summary>
	/// Adjust the offset by the amount of whitespace found after the current <paramref name="offset"/>
	/// </summary>
#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public static void Increment(ref this int offset, in int amount) => offset += amount;
}