using System;
using System.Runtime.CompilerServices;

namespace FluentSerializer.Core.Text.Extensions;

public static class OffsetExtensions
{
	/// <summary>
	/// Adjust the offset by the length of the provided token
	/// </summary>
#if NET6_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public static void AdjustForToken(ref this int offset, in ReadOnlySpan<char> token) =>
		offset += token.Length;

	/// <summary>
	/// Adjust the offset by the length of the provided token
	/// </summary>
	/// <remarks>
	/// This overload is mainly for readability since the length of a single char is always 1
	/// </remarks>
#if NET6_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public static void AdjustForToken(ref this int offset, in char token)
	{
		_ = token;
		offset++;
	}
}