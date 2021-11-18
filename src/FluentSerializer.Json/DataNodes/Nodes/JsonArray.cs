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
    [DebuggerDisplay("{ArrayName, nq}")]
    public readonly struct JsonArray : IJsonArray
    {
        private const string ArrayName = "[ ]";
        public string Name => ArrayName;

        private readonly IJsonNode? _lastNonCommentChild;
        private readonly List<IJsonNode> _children;
        public IReadOnlyList<IJsonNode> Children => _children ?? new List<IJsonNode>();

        public JsonArray(params IJsonArrayContent[] elements) : this(elements.AsEnumerable()) { }
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

        public JsonArray(ReadOnlySpan<char> text, ref int offset)
        {
            _children = new List<IJsonNode>();
            _lastNonCommentChild = null;

            offset++;

            while (offset < text.Length)
            {
                var character = text[offset];

                if (character == JsonConstants.ObjectStartCharacter)
                {
                    var jsonObject = new JsonObject(text, ref offset);
                    _children.Add(jsonObject);
                    _lastNonCommentChild = jsonObject;
                    continue;
                }
                if (character == JsonConstants.ArrayStartCharacter)
                {
                    var jsonArray = new JsonArray(text, ref offset);
                    _children.Add(jsonArray);
                    _lastNonCommentChild = jsonArray;
                    continue;
                }

                if (character == JsonConstants.PropertyWrapCharacter) break;
                if (character == JsonConstants.ObjectEndCharacter) break;
                if (character == JsonConstants.ArrayEndCharacter) break;

                if (text.HasStringAtOffset(offset, JsonConstants.SingleLineCommentMarker))
                {
                    _children.Add(new JsonCommentSingleLine(text, ref offset));
                    continue;
                }
                if (text.HasStringAtOffset(offset, JsonConstants.MultiLineCommentStart))
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
                .Append(JsonConstants.ArrayStartCharacter);
            
            foreach (var child in Children)
            {
                stringBuilder
                    .AppendOptionalNewline(format)
                    .AppendOptionalIndent(childIndent, format)
                    .AppendNode(child, format, childIndent, writeNull);

                // Make sure the last item does not append a comma to confirm to JSON spec.
                if (child is not IJsonComment && !child.Equals(_lastNonCommentChild)) 
                    stringBuilder.Append(JsonConstants.DividerCharacter);
            }

            stringBuilder
                .AppendOptionalNewline(format)
                .AppendOptionalIndent(indent, format)
                .Append(JsonConstants.ArrayEndCharacter);

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
