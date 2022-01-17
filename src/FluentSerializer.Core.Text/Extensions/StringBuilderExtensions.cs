using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Text.Writers;
using System.Text;

namespace FluentSerializer.Core.Text.Extensions;

public static class StringBuilderExtensions
{
	public static ITextWriter AppendNode(this ITextWriter stringBuilder, in IDataNode node, bool format, in int indent, in bool writeNull)
	{
		return node.AppendTo(ref stringBuilder, format, format ? indent : 0, writeNull);
	}
	
	public static ITextWriter AppendOptionalNewline(this ITextWriter stringBuilder, in bool newLine)
	{
		if (!newLine) return stringBuilder;

		return stringBuilder.AppendLineEnding();
	}
	public static ITextWriter AppendOptionalIndent(this ITextWriter stringBuilder, in int indent, in bool format)
	{
		const char indentChar = '\t';

		if (!format) return stringBuilder;
		return stringBuilder.Append(indentChar, indent);
	}
	
	public static string ToString(this IDataNode node, SerializerConfiguration configuration)
	{
		var stringBuilder = (ITextWriter)new SystemStringBuilder(configuration, new StringBuilder());
		stringBuilder = node.AppendTo(ref stringBuilder);
		return stringBuilder.ToString();
	}
}