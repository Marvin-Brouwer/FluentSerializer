using FluentSerializer.Core.DataNodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FluentSerializer.Json.DataNodes
{
    [DebuggerDisplay("{Name, nq}")]
    public readonly struct JsonObject : IJsonContainer, IEquatable<IJsonNode>
    {
        private readonly List<IJsonNode> _children;
        public IReadOnlyList<IJsonNode> Children => _children ?? new List<IJsonNode>();

        public string Name { get; }

        public JsonObject(IEnumerable<JsonProperty>? properties)
        {

            const string className = "{ }";
            Name = className;

            if (properties is null) _children = new List<IJsonNode>(0);
            else
            {
                _children = new List<IJsonNode>();
                foreach (var property in properties) _children.Add(property);
            }
        }

        public JsonObject(params JsonProperty[] properties) : this(properties.AsEnumerable()) { }
        public JsonObject(ReadOnlySpan<char> text, StringBuilder stringBuilder, ref int offset) : this((IEnumerable<JsonProperty>?)null)
        {
            const char propertyStartCharacter = '"';
            const char objectStartCharacter = '{';
            const char objectEndCharacter = '}';
            const char arrayStartCharacter = '[';
            const char arrayEndCharacter = ']';

            offset++;
            _children = new List<IJsonNode>();

            stringBuilder.Clear();
            while (offset < text.Length)
            {
                var character = text[offset];

                if (character == objectEndCharacter) break;
                if (character == arrayEndCharacter) break;

                if (character == objectStartCharacter)
                {
                    _children.Add(new JsonObject(text, stringBuilder, ref offset));
                    continue;
                }
                if (character == arrayStartCharacter)
                {
                    _children.Add(new JsonArray(text, stringBuilder, ref offset));
                    continue;
                }

                offset++;
                if (character == propertyStartCharacter)
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
            const char openingCharacter = '{';
            const char separatorCharacter = ',';
            const char closingCharacter = '}';

            var childIndent = indent + 1;

            stringBuilder
                .Append(openingCharacter);

            for (var i = 0; i < Children.Count; i++)
            {
                var child = Children[i];

                stringBuilder
                    .AppendOptionalNewline(format)
                    .AppendOptionalIndent(childIndent, format)
                    .AppendNode(child, format, childIndent);

                if (i != Children.Count -1) stringBuilder.Append(separatorCharacter);
            }

            stringBuilder
                .AppendOptionalNewline(format)
                .AppendOptionalIndent(indent, format)
                .Append(closingCharacter);

            return stringBuilder;
        }

        #region IEquatable

        public override bool Equals(object? obj)
        {
            if (obj is not IJsonNode jsonNode) return false;

            return Equals(jsonNode);
        }

        public bool Equals(IJsonNode? other)
        {
            if (other is not JsonObject otherObject) return false;

            return Children.SequenceEqual(otherObject.Children);
        }

        public override int GetHashCode() => HashCode.Combine(_children);

        #endregion
    }
}
