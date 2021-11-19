using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Extensions;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace FluentSerializer.Json.DataNodes.Nodes
{
    /// <inheritdoc cref="IJsonArray"/>
    [DebuggerDisplay("{ArrayName, nq}")]
    public readonly struct JsonArray : IJsonArray
    {
        private const string ArrayName = "[ ]";
        public string Name => ArrayName;

        private readonly IJsonNode? _lastNonCommentChild;
        private readonly List<IJsonNode> _children;
        public IReadOnlyList<IJsonNode> Children => _children ?? new List<IJsonNode>();

        /// <inheritdoc cref="JsonBuilder.Array(IJsonArrayContent[])"/>
        /// <remarks>
        /// <b>Please use <see cref="JsonBuilder.Array"/> method instead of this constructor</b>
        /// </remarks>
        public JsonArray(params IJsonArrayContent[] elements) : this(elements.AsEnumerable()) { }

        /// <inheritdoc cref="JsonBuilder.Array(IEnumerable{IJsonArrayContent}))"/>
        /// <remarks>
        /// <b>Please use <see cref="JsonBuilder.Array"/> method instead of this constructor</b>
        /// </remarks>
        public JsonArray(IEnumerable<IJsonArrayContent>? elements)
        {
            _lastNonCommentChild = null;

            if (elements is null)
            {
                _children = new List<IJsonNode>(0);
            }
            else
            {
                _children = new List<IJsonNode>();
                foreach (var property in elements)
                {
                    _children.Add(property);
                    if (property is not IJsonComment) _lastNonCommentChild = property;
                }
            }
        }

        /// <inheritdoc cref="IJsonArray"/>
        /// <remarks>
        /// <b>Please use <see cref="JsonParser.Parse"/> method instead of this constructor</b>
        /// </remarks>
        public JsonArray(ReadOnlySpan<char> text, ref int offset)
        {
            _children = new List<IJsonNode>();
            _lastNonCommentChild = null;

            offset++;

            while (offset < text.Length)
            {
                var character = text[offset];

                if (character == JsonCharacterConstants.ObjectStartCharacter)
                {
                    var jsonObject = new JsonObject(text, ref offset);
                    _children.Add(jsonObject);
                    _lastNonCommentChild = jsonObject;
                    continue;
                }
                if (character == JsonCharacterConstants.ArrayStartCharacter)
                {
                    var jsonArray = new JsonArray(text, ref offset);
                    _children.Add(jsonArray);
                    _lastNonCommentChild = jsonArray;
                    continue;
                }

                if (character == JsonCharacterConstants.PropertyWrapCharacter) break;
                if (character == JsonCharacterConstants.ObjectEndCharacter) break;
                if (character == JsonCharacterConstants.ArrayEndCharacter) break;

                if (text.HasStringAtOffset(offset, JsonCharacterConstants.SingleLineCommentMarker))
                {
                    _children.Add(new JsonCommentSingleLine(text, ref offset));
                    continue;
                }
                if (text.HasStringAtOffset(offset, JsonCharacterConstants.MultiLineCommentStart))
                {
                    _children.Add(new JsonCommentMultiLine(text, ref offset));
                    continue;
                }
                offset++;
            }
            offset++;
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

            stringBuilder = AppendTo(stringBuilder, format, indent, writeNull);
            writer.Write(stringBuilder);

            stringBuilder.Clear();
            stringBuilders.Return(stringBuilder);
        }

        public StringBuilder AppendTo(StringBuilder stringBuilder, bool format = true, int indent = 0, bool writeNull = true)
        {
            var childIndent = indent + 1;

            stringBuilder
                .Append(JsonCharacterConstants.ArrayStartCharacter);
            
            foreach (var child in Children)
            {
                stringBuilder
                    .AppendOptionalNewline(format)
                    .AppendOptionalIndent(childIndent, format)
                    .AppendNode(child, format, childIndent, writeNull);

                // Make sure the last item does not append a comma to confirm to JSON spec.
                if (child is not IJsonComment && !child.Equals(_lastNonCommentChild)) 
                    stringBuilder.Append(JsonCharacterConstants.DividerCharacter);
            }

            stringBuilder
                .AppendOptionalNewline(format)
                .AppendOptionalIndent(indent, format)
                .Append(JsonCharacterConstants.ArrayEndCharacter);

            return stringBuilder;
        }

        #region IEquatable

        public override bool Equals(object? obj)
        {
            if (obj is not IJsonNode node) return false;
            return Equals(node);
        }

        public bool Equals(IJsonNode? obj)
        {
            if (obj is not JsonArray otherArray) return false;

            return _children.SequenceEqual(otherArray._children, JsonNodeComparer.Default);
        }

        public override int GetHashCode()
        {
            if (!_children.Any()) return 0;

            var hash = new HashCode();
            foreach (var child in _children)
                hash.Add(child);

            return hash.ToHashCode();
        }

        #endregion
    }
}
