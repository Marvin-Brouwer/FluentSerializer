namespace FluentSerializer.Core.DataNodes
{
    public static class StringFastExtensions
    {
        public static ITextWriter AppendNode(this ITextWriter stringBuilder, in IDataNode node, bool format, in uint indent, in bool writeNull)
        {
            return node.AppendTo(ref stringBuilder, format, format ? indent : 0, writeNull);
        }
        public static ITextWriter AppendOptionalNewline(this ITextWriter stringBuilder, in bool newLine)
        {
            if (!newLine) return stringBuilder;

            return stringBuilder.AppendLineEnding();
        }
        public static ITextWriter AppendOptionalIndent(this ITextWriter stringBuilder, in uint indent, in bool format)
        {
            const char indentChar = '\t';

            if (!format) return stringBuilder;
            return stringBuilder.Append(indentChar, indent);
        }
    }
}
