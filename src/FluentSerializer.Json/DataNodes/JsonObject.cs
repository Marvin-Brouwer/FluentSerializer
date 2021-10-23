using FluentSerializer.Core.DataNodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FluentSerializer.Json.DataNodes
{
    [DebuggerDisplay("{ObjectName, nq}")]
    public readonly record struct JsonObject : IJsonContainer, IEquatable<IJsonNode>
    {
        private const string ObjectName = "[ ]";
        public string Name => ObjectName;

        private readonly List<IJsonNode> _children;
        public IReadOnlyList<IJsonNode> Children => _children ?? new List<IJsonNode>();

        public JsonObject(params JsonProperty[] properties) : this(properties.AsEnumerable()) { }
        public JsonObject(IEnumerable<JsonProperty>? properties)
        {
            if (properties is null) _children = new List<IJsonNode>(0);
            else
            {
                _children = new List<IJsonNode>();
                foreach (var property in properties) _children.Add(property);
            }
        }

        public JsonObject(ReadOnlySpan<char> text, StringBuilder stringBuilder, ref int offset)
        {
            _children = new List<IJsonNode>();

            offset++;

            stringBuilder.Clear();
            while (offset < text.Length)
            {
                var character = text[offset];

                if (character == JsonConstants.ObjectEndCharacter) break;
                if (character == JsonConstants.ArrayEndCharacter) break;

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

                offset++;
                if (character == JsonConstants.PropertyWrapCharacter)
                {
                    _children.Add(new JsonProperty(text, stringBuilder, ref offset));
                    continue;
                }
            }
            offset++;
        }

        public override string ToString() => ToString(false);
        public string ToString(bool format) => WriteTo(new StringBuilder(), format).ToString();
        public StringBuilder WriteTo(StringBuilder stringBuilder, bool format = true, int indent = 0, bool writeNull = true)
        {
            var childIndent = indent + 1;

            stringBuilder
                .Append(JsonConstants.ObjectStartCharacter);

            for (var i = 0; i < Children.Count; i++)
            {
                var child = Children[i];

                stringBuilder
                    .AppendOptionalNewline(format)
                    .AppendOptionalIndent(childIndent, format)
                    .AppendNode(child, format, childIndent);

                if (i != Children.Count -1) stringBuilder.Append(JsonConstants.DividerCharacter);
            }

            stringBuilder
                .AppendOptionalNewline(format)
                .AppendOptionalIndent(indent, format)
                .Append(JsonConstants.ObjectEndCharacter);

            return stringBuilder;
        }

        #region IEquatable

        public bool Equals(IJsonNode? other)
        {
            if (other is not JsonObject otherObject) return false;

            return _children.SequenceEqual(otherObject._children, JsonNodeComparer.Default);
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
