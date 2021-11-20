﻿using System.Diagnostics;
using Ardalis.GuardClauses;
using FluentSerializer.Core.Extensions;
using System.Text;
using System;
using Microsoft.Extensions.ObjectPool;
using System.IO;

namespace FluentSerializer.Xml.DataNodes.Nodes
{
    /// <inheritdoc cref="IXmlAttribute"/>
    [DebuggerDisplay("{Name,nq}={Value}")]
    public readonly struct XmlAttribute : IXmlAttribute
    {
        public string Name { get; }
        public string? Value { get; }

        /// <inheritdoc cref="XmlBuilder.Attribute(string, string?)"/>
        /// <remarks>
        /// <b>Please use <see cref="XmlBuilder.Attribute"/> method instead of this constructor</b>
        /// </remarks>
        public XmlAttribute(string name, string? value = null)
        {
            Guard.Against.InvalidName(name, nameof(name));

            Name = name;
            Value = value;
        }

        /// <inheritdoc cref="IXmlAttribute"/>
        /// <remarks>
        /// <b>Please use <see cref="XmlParser.Parse"/> method instead of this constructor</b>
        /// </remarks>
        public XmlAttribute(ReadOnlySpan<char> text, ref int offset)
        {
            var nameStartOffset = offset;
            var nameEndOffset = offset;

            while (offset < text.Length)
            {
                nameEndOffset = offset;

                var character = text[offset];

                if (character == XmlCharacterConstants.TagTerminationCharacter) break;
                if (character == XmlCharacterConstants.TagEndCharacter) break;
                offset++;
                if (character == XmlCharacterConstants.PropertyAssignmentCharacter) break;
            }

            Name = text[nameStartOffset..nameEndOffset].ToString().Trim();

            while (offset < text.Length)
            {
                var character = text[offset];

                if (character == XmlCharacterConstants.TagTerminationCharacter) break;
                if (character == XmlCharacterConstants.TagStartCharacter) break;
                if (character == XmlCharacterConstants.PropertyWrapCharacter)
                {
                    offset++;
                    break;
                }
                if (!char.IsWhiteSpace(character)) break;
                offset++;
            }
            
            var valueStartOffset = offset;
            var valueEndOffset = offset;

            while (offset < text.Length)
            {
                valueEndOffset = offset;

                var character = text[offset];

                if (character == XmlCharacterConstants.TagTerminationCharacter) break;
                if (character == XmlCharacterConstants.TagStartCharacter) break;
                offset++;
                if (character == XmlCharacterConstants.PropertyWrapCharacter) break;
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
                .Append(XmlCharacterConstants.PropertyAssignmentCharacter)
                .Append(XmlCharacterConstants.PropertyWrapCharacter);

            if (Value is not null) stringBuilder.Append(Value);

            stringBuilder
                .Append(XmlCharacterConstants.PropertyWrapCharacter);

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
