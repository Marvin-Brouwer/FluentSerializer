namespace FluentSerializer.Core.DataNodes
{
    public static class StringFastExtensions
    {
        public static StringFast AppendNode(this StringFast stringBuilder, IDataNode node, bool format, int indent, bool writeNull)
        {
            return node.AppendTo(stringBuilder, format, format ? indent : 0, writeNull);
        }
        public static StringFast AppendOptionalNewline(this StringFast stringBuilder, bool newLine)
        {
            if (!newLine) return stringBuilder;

            return stringBuilder.AppendLine();
        }
        public static StringFast AppendOptionalIndent(this StringFast stringBuilder, int indent, bool format)
        {
            const char indentChar = '\t';

            if (!format) return stringBuilder;
            return stringBuilder.Append(indentChar, indent);
        }
    }
}
