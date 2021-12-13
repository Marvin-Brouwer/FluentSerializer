using System;

namespace FluentSerializer.Core.Extensions;

public static class EquationExtensions
{
	public static bool EqualsString(this ReadOnlySpan<char> charSpan, string text) =>
		charSpan.Equals(text, StringComparison.OrdinalIgnoreCase);
	public static bool HasStringAtOffset(this ReadOnlySpan<char> charSpan, int offset, string text) =>
		charSpan[offset..(offset + text.Length)].EqualsString(text);
}