using Ardalis.GuardClauses;
using FluentSerializer.Core.Extensions;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace FluentSerializer.Json.DataNodes.Nodes
{
    [DebuggerDisplay("/* {Value,nq} */")]
    public readonly struct JsonCommentMultiLine : IJsonComment
    {
        public string Name => JsonConstants.SingleLineCommentMarker;
        public string? Value { get; }

        public JsonCommentMultiLine(string value)
        {
            Guard.Against.NullOrEmpty(value, nameof(value));

            Value = value;
        }

        public JsonCommentMultiLine(ReadOnlySpan<char> text, ref int offset)
        {
            offset += JsonConstants.MultiLineCommentStart.Length;

            var stringBuilder = new StringBuilder(128);
            while (offset < text.Length)
            {
                if (text.HasStringAtOffset(offset, JsonConstants.MultiLineCommentEnd)) 
                {
                    offset += JsonConstants.MultiLineCommentEnd.Length;
                    break;
                }

                var character = text[offset];
                offset++;

                stringBuilder.Append(character);
            }

            Value = stringBuilder.ToString().Trim();
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

            const char spacer = ' ';

            return stringBuilder
                .Append(JsonConstants.MultiLineCommentStart)
                .Append(spacer)
                .Append(Value)
                .Append(spacer)
                .Append(JsonConstants.MultiLineCommentEnd);
        }

        #region IEquatable

        public override bool Equals(object? obj)
        {
            if (obj is not IJsonNode node) return false;
            return Equals(node);
        }

        public bool Equals(IJsonNode? other)
        {
            if (other is not JsonCommentMultiLine otherComment) return false;
            if (Value is null && otherComment.Value is null) return true;
            if (otherComment.Value is null) return false;

            return Value!.Equals(otherComment.Value, StringComparison.Ordinal);
        }

        public override int GetHashCode() => Value?.GetHashCode() ?? 0;

        #endregion
    }
}
