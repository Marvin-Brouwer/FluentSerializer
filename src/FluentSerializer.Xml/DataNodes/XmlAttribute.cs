using System.Diagnostics;
using Ardalis.GuardClauses;
using FluentSerializer.Core.Extensions;
using System.Text;
using System;

namespace FluentSerializer.Xml.DataNodes
{
    [DebuggerDisplay("{Name,nq}={Value}")]
    public readonly struct XmlAttribute : IXmlValue, IEquatable<IXmlNode>
    {
        public string Name { get; }
        public string? Value { get; }

        public XmlAttribute(string name, string? value = null)
        {
            Guard.Against.InvalidName(name, nameof(name));

            Name = name;
            Value = value;
        }
        public XmlAttribute(ReadOnlySpan<char> text, StringBuilder stringBuilder, ref int offset)
        {
            const char valueEndCharacter = '"';
            const char valueAssignmentCharacter = '=';
            const char tagStartCharacter = '<';
            const char tagTerminateCharacter = '/';

            stringBuilder.Clear();
            while (offset < text.Length)
            {
                var character = text[offset];

                if (character == tagTerminateCharacter) break;
                if (character == tagStartCharacter) break;
                offset++;
                if (character == valueAssignmentCharacter) break;
                if (char.IsWhiteSpace(character)) continue;

                stringBuilder.Append(character);
            }

            Name = stringBuilder.ToString();
            stringBuilder.Clear();

            while (offset < text.Length)
            {
                var character = text[offset];

                if (character == tagTerminateCharacter) break;
                if (character == tagStartCharacter) break;
                if (character == valueEndCharacter)
                {
                    offset++;
                    break;
                }
                if (!char.IsWhiteSpace(character)) break;
                offset++;
            }

            while (offset < text.Length)
            {
                var character = text[offset];

                if (character == tagTerminateCharacter) break;
                if (character == tagStartCharacter) break;
                offset++;
                if (character == valueEndCharacter) break;

                stringBuilder.Append(character);
            }

            Value = stringBuilder.ToString();
        }

        public override string ToString() => ToString(false);
        public string ToString(bool format) => WriteTo(new StringBuilder(), format).ToString();
        public StringBuilder WriteTo(StringBuilder stringBuilder, bool format = true, int indent = 0, bool writeNull = true)
        {
            const char wrappingCharacter = '"';
            const char assignmentCharacter = '=';

            if (!writeNull && Value is null) return stringBuilder;

            stringBuilder
                .Append(Name)
                .Append(assignmentCharacter)
                .Append(wrappingCharacter)
                .Append(Value)
                .Append(wrappingCharacter);

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
            if (obj is not XmlAttribute otherAttribute) return false;

            return Name!.Equals(otherAttribute.Name, StringComparison.Ordinal)
                && Value?.Equals(otherAttribute.Value, StringComparison.Ordinal) == true;
        }
        public override int GetHashCode() => HashCode.Combine(Name, Value);

        #endregion
    }
}
