using Microsoft.Extensions.ObjectPool;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace FluentSerializer.Json.DataNodes.Nodes
{
    /// <inheritdoc cref="IJsonValue"/>
    [DebuggerDisplay("{Value,nq}")]
    public readonly struct JsonValue : IJsonValue
    {
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
            // JSON does not support empty property assignment or array members
            if (!writeNull && string.IsNullOrEmpty(Value)) return stringBuilder;

            return stringBuilder.Append(Value);
        }

        #region IEquatable

        public override bool Equals(object? obj)
        {
            if (obj is not IJsonNode node) return false;
            return Equals(node);
        }

        public bool Equals(IJsonNode? other)
        {
            if (other is not JsonValue otherValue) return false;
            if (Value is null && otherValue.Value is null) return true;
            if (otherValue.Value is null) return false;

            return Value!.Equals(otherValue.Value, StringComparison.Ordinal);
        }

        public override int GetHashCode() => Value?.GetHashCode() ?? 0;

        #endregion
    }
}
