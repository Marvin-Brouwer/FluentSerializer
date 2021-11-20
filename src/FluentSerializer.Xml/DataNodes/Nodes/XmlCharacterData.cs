using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Extensions;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Diagnostics;

namespace FluentSerializer.Xml.DataNodes.Nodes
{
    /// <inheritdoc cref="IXmlCharacterData"/>
    [DebuggerDisplay(CharacterDataName)]
    public readonly struct XmlCharacterData : IXmlCharacterData
    {
        private static readonly int TypeHashCode = typeof(XmlCharacterData).GetHashCode();

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
            var stringBuilder = new StringFast();
            stringBuilder = AppendTo(stringBuilder);
            return stringBuilder.ToString();
        }

        public string WriteTo(ObjectPool<StringFast> stringBuilders, bool format = true, bool writeNull = true, int indent = 0)
        {
            var stringBuilder = stringBuilders.Get();
            try
            {
                stringBuilder = AppendTo(stringBuilder, format, indent, writeNull);
                return stringBuilder.ToString();
            }
            finally
            {
                stringBuilders.Return(stringBuilder);
            }
        }

        public StringFast AppendTo(StringFast stringBuilder, bool format = true, int indent = 0, bool writeNull = true)
        {
            if (!writeNull && string.IsNullOrEmpty(Value)) return stringBuilder;

            return stringBuilder
                .Append(XmlCharacterConstants.CharacterDataStart)
                .Append(Value)
                .Append(XmlCharacterConstants.CharacterDataEnd);
        }

        #region IEquatable

        public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

        public bool Equals(IDataNode? other) => other is IXmlNode node && Equals(node);

        public bool Equals(IXmlNode? other) => DataNodeComparer.Default.Equals(this, other);

        public override int GetHashCode() => DataNodeComparer.Default.GetHashCodeForAll(TypeHashCode, Value);

        #endregion
    }
}
