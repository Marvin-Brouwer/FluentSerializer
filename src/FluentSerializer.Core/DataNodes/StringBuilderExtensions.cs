﻿using System.Text;

namespace FluentSerializer.Core.DataNodes
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendNode(this StringBuilder stringBuilder, IDataNode node, bool format, int indent, bool writeNull)
        {
            return node.AppendTo(stringBuilder, format, format ? indent : 0, writeNull);
        }
        public static StringBuilder AppendOptionalNewline(this StringBuilder stringBuilder, bool newLine)
        {
            if (!newLine) return stringBuilder;

            return stringBuilder.AppendLine();
        }
        public static StringBuilder AppendOptionalIndent(this StringBuilder stringBuilder, int indent, bool format)
        {
            const char indentChar = '\t';

            if (!format) return stringBuilder;
            return stringBuilder.Append(indentChar, indent);
        }
    }
}