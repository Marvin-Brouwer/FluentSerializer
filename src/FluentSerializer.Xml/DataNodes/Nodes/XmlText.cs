using FluentSerializer.Core.DataNodes;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace FluentSerializer.Xml.DataNodes.Nodes
{
    /// <inheritdoc cref="IXmlText"/>
    [DebuggerDisplay("{Value}")]
    public readonly struct XmlText : IXmlText
    {
        private static readonly int TypeHashCode = typeof(XmlText).GetHashCode();

        internal const string TextName = "#text";

        public string Name => TextName;
        public string? Value { get; }

        /// <inheritdoc cref="XmlBuilder.Text(string?)"/>
        /// <remarks>
        /// <b>Please use <see cref="XmlBuilder.Text"/> method instead of this constructor</b>
        /// </remarks>
        public XmlText(string? value = null)
        {
            Value = value;
        }

        /// <inheritdoc cref="IXmlText"/>
        /// <remarks>
        /// <b>Please use <see cref="XmlParser.Parse"/> method instead of this constructor</b>
        /// </remarks>
        public XmlText(ReadOnlySpan<char> text, ref int offset)
        {
            var valueStartOffset = offset;
            var valueEndOffset = offset;

            while (offset < text.Length)
            {
                valueEndOffset = offset;

                var character = text[offset];
                if (character == XmlCharacterConstants.TagStartCharacter) break;

                offset++;
            }

            Value = text[valueStartOffset..valueEndOffset].ToString().Trim();
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
            // This should never happen because null tags are self-closing but just to be sure this check is here
            if (!writeNull && string.IsNullOrEmpty(Value)) return stringBuilder;

            return stringBuilder.Append(Value);
        }

        #region IEquatable

        public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

        public bool Equals(IDataNode? other) => other is IXmlNode node && Equals(node);

        public bool Equals(IXmlNode? other) => DataNodeComparer.Default.Equals(this, other);

        public override int GetHashCode() => DataNodeComparer.Default.GetHashCodeForAll(TypeHashCode, Value);

        #endregion
    }
}
