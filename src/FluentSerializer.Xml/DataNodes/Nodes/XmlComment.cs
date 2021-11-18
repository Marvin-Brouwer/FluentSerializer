using FluentSerializer.Core.Extensions;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace FluentSerializer.Xml.DataNodes.Nodes
{
    [DebuggerDisplay("<!-- {Value, nq} -->")]
    public readonly struct XmlComment : IXmlComment
    {
        private const string CommentName = "<!-- comment -->";
        public string Name => CommentName;
        public string? Value { get; }

        public XmlComment(string? value = null)
        {
            Value = value;
        }

        // todo maybe just store the span with offset and range instead of allocating a new stringbuilder
        public XmlComment(ReadOnlySpan<char> text, ref int offset)
        {
            offset += XmlConstants.CommentStart.Length;

            var stringBuilder = new StringBuilder(128);
            while (offset < text.Length)
            {
                if (text.HasStringAtOffset(offset, XmlConstants.CommentEnd))
                {
                    offset += XmlConstants.CommentEnd.Length;
                    break;
                }

                var character = text[offset];
                offset++;

                stringBuilder.Append(character);
            }

            Value = stringBuilder.ToString().Trim();
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder = AppendTo(stringBuilder);
            return stringBuilder.ToString();
        }

        public void WriteTo(ObjectPool<StringBuilder> stringBuilders, TextWriter writer, bool format = true, bool writeNull = true, int indent = 0)
        {
            var stringBuilder = stringBuilders.Get();
            writer.Write(AppendTo(stringBuilder, format, indent, writeNull));
            stringBuilders.Return(stringBuilder);
        }

        public StringBuilder AppendTo(StringBuilder stringBuilder, bool format = true, int indent = 0, bool writeNull = true)
        {
            if (!writeNull && string.IsNullOrEmpty(Value)) return stringBuilder;

            const char spacer = ' ';

            return stringBuilder
                .Append(XmlConstants.CommentStart)
                .Append(spacer)
                .Append(Value)
                .Append(spacer)
                .Append(XmlConstants.CommentEnd);
        }

        #region IEquatable

        public override bool Equals(object? obj)
        {
            if (obj is not IXmlNode xmlNode) return false;

            return Equals(xmlNode);
        }

        public bool Equals(IXmlNode? obj)
        {
            if (obj is not XmlComment otherComment) return false;
            if (Value is null && otherComment.Value is null) return true;
            if (otherComment.Value is null) return false;

            return Value!.Equals(otherComment.Value, StringComparison.Ordinal);
        }

        public override int GetHashCode() => HashCode.Combine(Name, Value);

        #endregion
    }
}
