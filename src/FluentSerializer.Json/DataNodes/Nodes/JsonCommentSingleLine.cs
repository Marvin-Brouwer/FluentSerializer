using Ardalis.GuardClauses;
using FluentSerializer.Core.Constants;
using FluentSerializer.Core.DataNodes;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Diagnostics;

namespace FluentSerializer.Json.DataNodes.Nodes
{
    /// <inheritdoc cref="IJsonComment"/>
    [DebuggerDisplay("// {Value,nq}")]
    public readonly struct JsonCommentSingleLine : IJsonComment
    {
        private static readonly int TypeHashCode = typeof(JsonCommentSingleLine).GetHashCode();

        public string Name => JsonCharacterConstants.SingleLineCommentMarker;
        public string? Value { get; }

        /// <inheritdoc cref="JsonBuilder.Comment(string)"/>
        /// <remarks>
        /// <b>Please use <see cref="JsonBuilder.Comment"/> method instead of this constructor</b>
        /// </remarks>
        public JsonCommentSingleLine(string value)
        {
            Guard.Against.NullOrEmpty(value, nameof(value));

            Value = value;
        }

        /// <inheritdoc cref="IJsonComment"/>
        /// <remarks>
        /// <b>Please use <see cref="JsonParser.Parse"/> method instead of this constructor</b>
        /// </remarks>
        public JsonCommentSingleLine(ReadOnlySpan<char> text, ref int offset)
        {
            offset += JsonCharacterConstants.SingleLineCommentMarker.Length;

            var valueStartOffset = offset;
            var valueEndOffset = offset;

            while (offset < text.Length)
            {
                valueEndOffset = offset;
                var character = text[offset];
                offset++;

                if (character == JsonCharacterConstants.LineReturnCharacter) break;
                if (character == JsonCharacterConstants.NewLineCharacter) break;
            }

            Value = text[valueStartOffset..valueEndOffset].ToString().Trim();
		}
		public override string ToString() => ((IDataNode)this).ToString(LineEndings.Environment);

		public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in uint indent = 0, in bool writeNull = true)
		{
			// JSON does not support empty property assignment or array members
			if (!writeNull && string.IsNullOrEmpty(Value)) return stringBuilder;

            const char spacer = ' ';

            // Fallback because otherwise JSON wouldn't be readable
            if (!format)
                return stringBuilder = stringBuilder
					.Append(JsonCharacterConstants.MultiLineCommentStart)
					.Append(spacer)
					.Append(Value)
					.Append(spacer)
					.Append(JsonCharacterConstants.MultiLineCommentEnd);

            return stringBuilder = stringBuilder
				.Append(JsonCharacterConstants.SingleLineCommentMarker)
                .Append(spacer)
                .Append(Value);
        }

        #region IEquatable

        public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

        public bool Equals(IDataNode? other) => other is IJsonNode node && Equals(node);

        public bool Equals(IJsonNode? other) => DataNodeComparer.Default.Equals(this, other);

        public override int GetHashCode() => DataNodeComparer.Default.GetHashCodeForAll(TypeHashCode, Value);

        #endregion
    }
}
