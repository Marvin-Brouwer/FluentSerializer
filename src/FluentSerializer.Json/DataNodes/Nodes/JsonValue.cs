using FluentSerializer.Core.DataNodes;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Diagnostics;

namespace FluentSerializer.Json.DataNodes.Nodes
{
    /// <inheritdoc cref="IJsonValue"/>
    [DebuggerDisplay("{Value,nq}")]
    public readonly struct JsonValue : IJsonValue
    {
        private static readonly int TypeHashCode = typeof(JsonValue).GetHashCode();

        private const string ValueName = "#value";
        public string Name => ValueName;
        public string? Value { get; }

        /// <inheritdoc cref="JsonBuilder.Value(string?)"/>
        /// <remarks>
        /// <b>Please use <see cref="JsonBuilder.Value"/> method instead of this constructor</b>
        /// </remarks>
        public JsonValue(string? value)
        {
            Value = value;
        }

        /// <inheritdoc cref="IJsonValue"/>
        /// <remarks>
        /// <b>Please use <see cref="JsonParser.Parse"/> method instead of this constructor</b>
        /// </remarks>
        public JsonValue(ReadOnlySpan<char> text, ref int offset)
        {
            var stringValue = false;

            var valueStartOffset = offset;
            var valueEndOffset = offset;

            while (offset < text.Length)
            {
                valueEndOffset = offset;

                var character = text[offset];
                offset++;

                if (character == JsonCharacterConstants.PropertyWrapCharacter && stringValue) break; 
                if (character == JsonCharacterConstants.DividerCharacter) break;
                if (character == JsonCharacterConstants.ObjectEndCharacter) break;
                if (character == JsonCharacterConstants.ArrayEndCharacter) break;

                if (character == JsonCharacterConstants.PropertyWrapCharacter) stringValue = true; 
                if (!stringValue && char.IsWhiteSpace(character)) break;
            }

            // Append a '"' if it started with a '"'
            if (stringValue) valueEndOffset++;
            Value = text[valueStartOffset..valueEndOffset].ToString().Trim();
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
            // JSON does not support empty property assignment or array members
            if (!writeNull && string.IsNullOrEmpty(Value)) return stringBuilder;

            return stringBuilder.Append(Value);
        }

        #region IEquatable

        public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

        public bool Equals(IDataNode? other) => other is IJsonNode node && Equals(node);

        public bool Equals(IJsonNode? other) => DataNodeComparer.Default.Equals(this, other);

        public override int GetHashCode() => DataNodeComparer.Default.GetHashCodeForAll(TypeHashCode, Value);

        #endregion
    }
}
