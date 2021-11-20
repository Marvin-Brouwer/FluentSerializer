using Ardalis.GuardClauses;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Extensions;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Diagnostics;

namespace FluentSerializer.Json.DataNodes.Nodes
{
    /// <inheritdoc cref="IJsonComment"/>
    [DebuggerDisplay("/* {Value,nq} */")]
    public readonly struct JsonCommentMultiLine : IJsonComment
    {
        private static readonly int TypeHashCode = typeof(JsonCommentMultiLine).GetHashCode();

        public string Name => JsonCharacterConstants.SingleLineCommentMarker;
        public string? Value { get; }

        /// <inheritdoc cref="JsonBuilder.MultilineComment(string)"/>
        /// <remarks>
        /// <b>Please use <see cref="JsonBuilder.MultilineComment"/> method instead of this constructor</b>
        /// </remarks>
        public JsonCommentMultiLine(string value)
        {
            Guard.Against.NullOrEmpty(value, nameof(value));

            Value = value;
        }

        /// <inheritdoc cref="IJsonComment"/>
        /// <remarks>
        /// <b>Please use <see cref="JsonParser.Parse"/> method instead of this constructor</b>
        /// </remarks>
        public JsonCommentMultiLine(ReadOnlySpan<char> text, ref int offset)
        {
            offset += JsonCharacterConstants.MultiLineCommentStart.Length;

            var valueStartOffset = offset;
            var valueEndOffset = offset;

            while (offset < text.Length)
            {
                valueEndOffset = offset;

                if (text.HasStringAtOffset(offset, JsonCharacterConstants.MultiLineCommentEnd)) 
                {
                    offset += JsonCharacterConstants.MultiLineCommentEnd.Length;
                    break;
                }
                
                offset++;
            }

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

            const char spacer = ' ';

            return stringBuilder
                .Append(JsonCharacterConstants.MultiLineCommentStart)
                .Append(spacer)
                .Append(Value)
                .Append(spacer)
                .Append(JsonCharacterConstants.MultiLineCommentEnd);
        }

        #region IEquatable

        public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

        public bool Equals(IDataNode? other) => other is IJsonNode node && Equals(node);

        public bool Equals(IJsonNode? other) => DataNodeComparer.Default.Equals(this, other);

        public override int GetHashCode() => DataNodeComparer.Default.GetHashCodeForAll(TypeHashCode, Value);

        #endregion
    }
}
