using FluentSerializer.Core.DataNodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FluentSerializer.Json.DataNodes.Nodes
{
    [DebuggerDisplay("{ArrayName, nq}")]
    internal readonly struct JsonArray : IJsonArray
    {
        private const string ArrayName = "[ ]";
        public string Name => ArrayName;


        private readonly List<IJsonNode> _children;
        public IReadOnlyList<IJsonNode> Children => _children ?? new List<IJsonNode>();

        public JsonArray(params IJsonContainer[] elements) : this(elements.AsEnumerable()) { }
        public JsonArray(IEnumerable<IJsonContainer>? elements)
        {
            if (elements is null) _children = new List<IJsonNode>(0);
            else
            {
                _children = new List<IJsonNode>();
                foreach (var property in elements) _children.Add(property);
            }
        }

        public JsonArray(ReadOnlySpan<char> text, StringBuilder stringBuilder, ref int offset)
        {
            _children = new List<IJsonNode>();

            offset++;

            stringBuilder.Clear();
            while (offset < text.Length)
            {
                var character = text[offset];

                if (character == JsonConstants.ObjectStartCharacter)
                {
                    _children.Add(new JsonObject(text, stringBuilder, ref offset));
                    continue;
                }
                if (character == JsonConstants.ArrayStartCharacter)
                {
                    _children.Add(new JsonArray(text, stringBuilder, ref offset));
                    continue;
                }


                if (character == JsonConstants.PropertyWrapCharacter) break;
                if (character == JsonConstants.ObjectEndCharacter) break;
                if (character == JsonConstants.ArrayEndCharacter) break;
                offset++;
            }
            offset++;
        }

        public override string ToString() => ToString(false);
        public string ToString(bool format) => WriteTo(new StringBuilder(), format).ToString();
        public StringBuilder WriteTo(StringBuilder stringBuilder, bool format = true, int indent = 0, bool writeNull = true)
        {
            var childIndent = indent + 1;

            stringBuilder
                .Append(JsonConstants.ArrayStartCharacter);

            for (var i = 0; i < Children.Count; i++)
            {
                var child = Children[i];

                stringBuilder
                    .AppendOptionalNewline(format)
                    .AppendOptionalIndent(childIndent, format)
                    .AppendNode(child, format, childIndent);

                if (i != Children.Count - 1) stringBuilder.Append(JsonConstants.DividerCharacter);
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
            if (_children?.Any() != true) return 0;

            var hash = new HashCode();
            foreach (var child in _children)
                hash.Add(child);

            return hash.ToHashCode();
        }

        #endregion
    }
}
