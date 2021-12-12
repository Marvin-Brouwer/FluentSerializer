using System.Diagnostics;
using Ardalis.GuardClauses;
using FluentSerializer.Core.Extensions;
using System;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Xml.Configuration;

namespace FluentSerializer.Xml.DataNodes.Nodes
{
	/// <inheritdoc cref="IXmlAttribute"/>
	[DebuggerDisplay("{Name,nq}={Value}")]
    public readonly struct XmlAttribute : IXmlAttribute
    {
        private static readonly int TypeHashCode = typeof(XmlAttribute).GetHashCode();

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

		public override string ToString() => ((IDataNode)this).ToString(XmlSerializerConfiguration.Default);

		public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
		{
			Guard.Against.NullOrWhiteSpace(Name, nameof(Name), "The attribute was is an illegal state, it contains no Name");

            if (!writeNull && Value is null) return stringBuilder;

			stringBuilder
				.Append(Name)
                .Append(XmlCharacterConstants.PropertyAssignmentCharacter)
                .Append(XmlCharacterConstants.PropertyWrapCharacter);

            if (Value is not null) stringBuilder = stringBuilder.Append(Value);

			stringBuilder
				.Append(XmlCharacterConstants.PropertyWrapCharacter);

            return stringBuilder;
        }

        #region IEquatable

        public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

        public bool Equals(IDataNode? other) => other is IXmlNode node && Equals(node);

        public bool Equals(IXmlNode? other) => DataNodeComparer.Default.Equals(this, other);

        public override int GetHashCode() => DataNodeComparer.Default.GetHashCodeForAll(TypeHashCode, Name, Value);

        #endregion
    }
}
