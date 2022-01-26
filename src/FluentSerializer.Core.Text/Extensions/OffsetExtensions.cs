using System;
using System.Runtime.CompilerServices;

namespace FluentSerializer.Core.Text.Extensions;

public static class OffsetExtensions
{
#if NET6_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public static void AdjustForToken(ref this int offset, in ReadOnlySpan<char> token) =>
		offset += token.Length;
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