using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace FluentSerializer.Json.DataNodes
{
    [DebuggerDisplay("{Value,nq}")]
    public readonly struct JsonValue : IJsonNode, IEquatable<IJsonNode>
    {
        public string Name { get; }
        public string? Value { get; }

        public static JsonValue String(string? value = null) => new ($"\"{value}\"");


        public JsonValue(string? value = null)
        {
            const string valueName = "#value";
            Name = valueName;
            Value = value;
        }

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

        public override bool Equals(object? obj)
        {
            if (obj is not IJsonNode node) return false;
            return Equals(node);
        }
        public bool Equals([AllowNull] IJsonNode other)
        {
            if (other is not JsonValue otherValue) return false;
            if (Value is null && other is null) return true;

            return Value!.Equals(otherValue.Value, StringComparison.Ordinal);
        }
    }
}
