using FluentSerializer.Core.Extensions;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace FluentSerializer.Xml.DataNodes.Nodes
{
    /// <inheritdoc cref="IXmlCharacterData"/>
    [DebuggerDisplay(CharacterDataName)]
    public readonly struct XmlCharacterData : IXmlCharacterData
    {
        private const string CharacterDataName = "<![CDATA[ ]]>";
        public string Name => CharacterDataName;
        public string? Value { get; }

        /// <inheritdoc cref="XmlBuilder.CData(string)"/>
        /// <remarks>
        /// <b>Please use <see cref="XmlBuilder.CData"/> method instead of this constructor</b>
        /// </remarks>
        public XmlCharacterData(string? value = null)
        {
            Value = value;
        }

        /// <inheritdoc cref="IXmlCharacterData"/>
        /// <remarks>
        /// <b>Please use <see cref="XmlParser.Parse"/> method instead of this constructor</b>
        /// </remarks>
        public XmlCharacterData(ReadOnlySpan<char> text, ref int offset)
        {
            offset += XmlCharacterConstants.CharacterDataStart.Length;

            var valueStartOffset = offset;
            var valueEndOffset = offset;
            
            while (offset < text.Length)
            {
                valueEndOffset = offset;
                if (text.HasStringAtOffset(offset, XmlCharacterConstants.CharacterDataEnd))
                {
                    offset += XmlCharacterConstants.CharacterDataEnd.Length;
                    break;
                }
                
                offset++;
            }

            Value = text[valueStartOffset..valueEndOffset].ToString();
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

            return stringBuilder
                .Append(XmlCharacterConstants.CharacterDataStart)
                .Append(Value)
                .Append(XmlCharacterConstants.CharacterDataEnd);
        }

        #region IEquatable

        public override bool Equals(object? obj)
        {
            if (obj is not IXmlNode xmlNode) return false;

            return Equals(xmlNode);
        }

        public bool Equals(IXmlNode? obj)
        {
            if (obj is not XmlCharacterData otherData) return false;
            if (Value is null && otherData.Value is null) return true;
            if (otherData.Value is null) return false;

            return Value!.Equals(otherData.Value, StringComparison.Ordinal);
        }

        public override int GetHashCode() => HashCode.Combine(Name, Value);

        #endregion
    }
}
