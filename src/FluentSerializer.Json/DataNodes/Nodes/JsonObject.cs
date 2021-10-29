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
    [DebuggerDisplay("{ObjectName, nq}")]
    public readonly struct JsonObject : IJsonObject
    {
        private const string ObjectName = "{ }";
        public string Name => ObjectName;

        private readonly List<IJsonNode> _children;
        public IReadOnlyList<IJsonNode> Children => _children ?? new List<IJsonNode>();

        public JsonObject(params IJsonObjectContent[] properties) : this(properties.AsEnumerable()) { }
        public JsonObject(IEnumerable<IJsonObjectContent>? properties)
        {
            if (properties is null) _children = new List<IJsonNode>(0);
            else
            {
                _children = new List<IJsonNode>();
                foreach (var property in properties) _children.Add(property);
            }
        }

        public JsonObject(ReadOnlySpan<char> text, ref int offset)
        {
            _children = new List<IJsonNode>();

            offset++;
            while (offset < text.Length)
            {
                var character = text[offset];

                if (character == JsonConstants.ObjectEndCharacter) break;
                if (character == JsonConstants.ArrayEndCharacter) break;

                if (character == JsonConstants.ObjectStartCharacter)
                {
                    _children.Add(new JsonObject(text, ref offset));
                    continue;
                }
                if (character == JsonConstants.ArrayStartCharacter)
                {
                    _children.Add(new JsonArray(text, ref offset));
                    continue;
                }

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
                if (character == JsonConstants.PropertyWrapCharacter)
                {
                    _children.Add(new JsonProperty(text, ref offset));
                    continue;
                }
            }
            offset++;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder = AppendTo(stringBuilder);
            return stringBuilder.ToString();
        }

        public void WriteTo(ObjectPool<StringBuilder> stringBuilders, TextWriter writer, bool format = true, int indent = 0, bool writeNull = true)
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
                .Append(JsonConstants.ObjectStartCharacter);

            for (var i = 0; i < Children.Count; i++)
            {
                var child = Children[i];

                stringBuilder
                    .AppendOptionalNewline(format)
                    .AppendOptionalIndent(childIndent, format)
                    .AppendNode(child, format, childIndent, writeNull);

                // todo figure out how to handle the last element followed by comments
                if (i != Children.Count - 1 && child is not IJsonComment) stringBuilder.Append(JsonConstants.DividerCharacter);
            }

            stringBuilder
                .AppendOptionalNewline(format)
                .AppendOptionalIndent(indent, format)
                .Append(JsonConstants.ObjectEndCharacter);

            return stringBuilder;
        }

        #region IEquatable

        public override bool Equals(object? obj)
        {
            if (obj is not IJsonNode node) return false;
            return Equals(node);
        }

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
