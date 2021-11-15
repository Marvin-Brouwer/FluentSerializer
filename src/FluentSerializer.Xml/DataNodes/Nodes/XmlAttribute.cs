using System.Diagnostics;
using Ardalis.GuardClauses;
using FluentSerializer.Core.Extensions;
using System.Text;
using System;
using Microsoft.Extensions.ObjectPool;
using System.IO;

namespace FluentSerializer.Xml.DataNodes.Nodes
{
    [DebuggerDisplay("{Name,nq}={Value}")]
    public readonly struct XmlAttribute : IXmlAttribute
    {
        public string Name { get; }
        public string? Value { get; }

        public XmlAttribute(string name, string? value = null)
        {
            Guard.Against.InvalidName(name, nameof(name));

            Name = name;
            Value = value;
        }
        public XmlAttribute(ReadOnlySpan<char> text, ref int offset)
        {
            var stringBuilder = new StringBuilder(128);
            while (offset < text.Length)
            {
                var character = text[offset];

                if (character == XmlConstants.TagTerminationCharacter) break;
                if (character == XmlConstants.TagEndCharacter) break;
                offset++;
                if (character == XmlConstants.PropertyAssignmentCharacter) break;
                if (char.IsWhiteSpace(character)) continue;

                stringBuilder.Append(character);
            }

            Name = stringBuilder.ToString();
            stringBuilder.Clear();

            while (offset < text.Length)
            {
                var character = text[offset];

                if (character == XmlConstants.TagTerminationCharacter) break;
                if (character == XmlConstants.TagStartCharacter) break;
                if (character == XmlConstants.PropertyWrapCharacter)
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

                if (character == XmlConstants.TagTerminationCharacter) break;
                if (character == XmlConstants.TagStartCharacter) break;
                offset++;
                if (character == XmlConstants.PropertyWrapCharacter) break;

                stringBuilder.Append(character);
            }

            Value = stringBuilder.ToString();
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder = AppendTo(stringBuilder);
            return stringBuilder.ToString();
        }

        public void WriteTo(ObjectPool<StringBuilder> stringBuilders, TextWriter writer, bool format = true, int indent = 0, bool writeNull = true)
        {
            Guard.Against.NullOrWhiteSpace(Name, nameof(Name), "The property was is an illegal state, it contains no Name");

            var stringBuilder = stringBuilders.Get();

            stringBuilder = AppendTo(stringBuilder, format, indent, writeNull);
            writer.Write(stringBuilder);

            stringBuilder.Clear();
            stringBuilders.Return(stringBuilder);
        }

        public StringBuilder AppendTo(StringBuilder stringBuilder, bool format = true, int indent = 0, bool writeNull = true)
        {
            Guard.Against.NullOrWhiteSpace(Name, nameof(Name), "The attribute was is an illegal state, it contains no Name");

            if (!writeNull && Value is null) return stringBuilder;

            stringBuilder
                .Append(Name)
                .Append(XmlConstants.PropertyAssignmentCharacter)
                .Append(XmlConstants.PropertyWrapCharacter);

            if (Value is not null) stringBuilder.Append(Value);

            stringBuilder
                .Append(XmlConstants.PropertyWrapCharacter);

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

            return Name.Equals(otherAttribute.Name, StringComparison.Ordinal)
                && Value?.Equals(otherAttribute.Value, StringComparison.Ordinal) == true;
        }
        public override int GetHashCode() => HashCode.Combine(Name, Value);

        #endregion
    }
}
