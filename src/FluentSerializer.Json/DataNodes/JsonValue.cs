﻿using System;
using System.Diagnostics;
using System.Text;

namespace FluentSerializer.Json.DataNodes
{
    [DebuggerDisplay("{Value,nq}")]
    public readonly record struct JsonValue(string? Value = null) : IJsonValue, IEquatable<IJsonNode>
    {
        private const string ValueName = "#value";
        public string Name => ValueName;
        public string? Value { get; } = Value;

        public static JsonValue String(string? value = null) => new ($"\"{value}\"");

        public JsonValue(ReadOnlySpan<char> text, StringBuilder stringBuilder, ref int offset) : this(null)
        {
            const char lineEndCharacter = ',';
            const char objectEndCharacter = '}';
            const char arrayEndCharacter = ']';
            const char stringWrapCharacter = '"';

            var stringValue = false;
            stringBuilder.Clear();
            while (offset < text.Length)
            {
                var character = text[offset];
                offset++;

                if (character == stringWrapCharacter && stringValue)
                {
                    stringBuilder.Append(character);
                    break;
                }
                if (character == lineEndCharacter) break;
                if (character == objectEndCharacter) break;
                if (character == arrayEndCharacter) break;

                if (character == stringWrapCharacter) stringValue = true; 
                if (!stringValue && char.IsWhiteSpace(character)) break;

                stringBuilder.Append(character);
            }

            Value = stringBuilder.ToString();
        }

        public override string ToString() => ToString(false);
        public string ToString(bool format) => WriteTo(new StringBuilder(), format).ToString();
        public StringBuilder WriteTo(StringBuilder stringBuilder, bool format = true, int indent = 0, bool writeNull = true)
        {
            // JSON does not support empty property assignment or array members
            if (!writeNull && string.IsNullOrEmpty(Value)) return stringBuilder;

           return stringBuilder.Append(Value);
        }

        #region IEquatable
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
