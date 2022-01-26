using System.Runtime.CompilerServices;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Text.Writers;
using System.Text;

namespace FluentSerializer.Core.Text.Extensions;

public static class StringBuilderExtensions
{
#if NET6_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public static ITextWriter AppendNode(this ITextWriter stringBuilder, in IDataNode node, in bool format, in int indent, in bool writeNull)
	{
		return node.AppendTo(ref stringBuilder, format, format ? indent : 0, writeNull);
	}

#if NET6_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public static ITextWriter AppendOptionalNewline(this ITextWriter stringBuilder, in bool newLine)
	{
		if (!newLine) return stringBuilder;

		return stringBuilder.AppendLineEnding();
	}
#if NET6_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public static ITextWriter AppendOptionalIndent(this ITextWriter stringBuilder, in int indent, in bool format)
	{
		const char indentChar = '\t';

		if (!format) return stringBuilder;
		return stringBuilder.Append(indentChar, indent);
	}

#if NET6_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public static string ToString(this IDataNode node, in SerializerConfiguration configuration)
	{
		var stringBuilder = (ITextWriter)new SystemStringBuilder(configuration, new StringBuilder());
		stringBuilder = node.AppendTo(ref stringBuilder);
		return stringBuilder.ToString();
	}
}