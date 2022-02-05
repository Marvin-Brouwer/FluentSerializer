using System.Runtime.CompilerServices;
using System.Text;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Text;

namespace FluentSerializer.Core.Extensions;

/// <summary>
/// Extensions for the <see cref="ITextWriter"/> specifically for appending <see cref="IDataNode"/>s serialized values
/// </summary>
public static class TextWriterExtensions
{
	/// <summary>
	/// Append this node's text content to the current text
	/// </summary>
#if NET6_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public static ITextWriter AppendNode(this ITextWriter stringBuilder, in IDataNode node, in bool format, in int indent, in bool writeNull)
	{
		return node.AppendTo(ref stringBuilder, format, format ? indent : 0, writeNull);
	}

	/// <summary>
	/// Append a new line when <paramref name="newLine"/> is set to true
	/// </summary>
	/// <remarks>
	/// This is mainly useful for chaining calls
	/// </remarks>
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

	/// <summary>
	/// Append an indent the amount of times specified by <paramref name="indent"/>
	/// when <paramref name="format"/> is set to true
	/// </summary>
	/// <remarks>
	/// This is mainly useful for chaining calls
	/// </remarks>
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

	/// <summary>
	/// ToString override by extension method because readonly structs don't support base classes
	/// </summary>
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