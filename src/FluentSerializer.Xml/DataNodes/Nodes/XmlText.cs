using Microsoft.Extensions.ObjectPool;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace FluentSerializer.Xml.DataNodes.Nodes
{
    [DebuggerDisplay("{Value}")]
    public readonly struct XmlText : IXmlText
    {

        public string Name { get; }
        public string? Value { get; }

        public XmlText(string? value = null)
        {
            const string textName = "#text";
            Name = textName;
            Value = value;
        }

        // todo maybe just store the span with offset and range instead of allocating a new stringbuilder
        public XmlText(ReadOnlySpan<char> text, ref int offset) : this(null)
        {
            // todo support CData, possibly in Element instead of here?
            var stringBuilder = new StringBuilder(128);
            while (offset < text.Length)
            {
                var character = text[offset];
                if (character == XmlConstants.TagStartCharacter) break;
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

        public void WriteTo(ObjectPool<StringBuilder> stringBuilders, TextWriter writer, bool format = true, int indent = 0, bool writeNull = true)
        {
            var stringBuilder = stringBuilders.Get();
            writer.Write(AppendTo(stringBuilder, format, indent, writeNull));
            stringBuilders.Return(stringBuilder);
        }

        public StringBuilder AppendTo(StringBuilder stringBuilder, bool format = true, int indent = 0, bool writeNull = true)
        {
            // THis should never happen because null tags are selfclosing but just to be sure this check is here
            if (!writeNull && string.IsNullOrEmpty(Value)) return stringBuilder;

            return stringBuilder.Append(Value);
        }

        #region IEquatable

        public override bool Equals(object? obj)
        {
            if (obj is not IXmlNode xmlNode) return false;

            return Equals(xmlNode);
        }

        public bool Equals(IXmlNode? obj)
        {
            if (obj is not XmlText otherText) return false;
            if (Value is null && otherText.Value is null) return true;
            if (otherText.Value is null) return false;

            return Value!.Equals(otherText.Value, StringComparison.Ordinal);
        }

        public override int GetHashCode() => HashCode.Combine(Name, Value);

        #endregion
    }
}
