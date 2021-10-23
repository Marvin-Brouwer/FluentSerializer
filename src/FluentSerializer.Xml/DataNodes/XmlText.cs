using System;
using System.Diagnostics;
using System.Text;

namespace FluentSerializer.Xml.DataNodes
{
    [DebuggerDisplay("{Value}")]
    public readonly struct XmlText : IXmlValue, IEquatable<IXmlNode>
    {

        public string Name { get; }
        public string? Value { get; }

        public XmlText(string? value = null)
        {
            const string textName = "#text";
            Name = textName;
            Value = value;
        }

        public XmlText(ReadOnlySpan<char> text, StringBuilder stringBuilder, ref int offset) : this(null)
        {
            const char tagStartCharacter = '<';

            stringBuilder.Clear();
            while (offset < text.Length)
            {
                var character = text[offset];
                if (character == tagStartCharacter) break;
                offset++;

                stringBuilder.Append(character);
            }

            Value = stringBuilder.ToString().Trim();
        }

        public override string ToString() => ToString(false);
        public string ToString(bool format) => WriteTo(new StringBuilder(), format).ToString();
        public StringBuilder WriteTo(StringBuilder stringBuilder, bool format = true, int indent = 0, bool writeNull = true)
        {
            if (!writeNull && Value is null) return stringBuilder;

            stringBuilder.Append(Value);

            return stringBuilder;
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
