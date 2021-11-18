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


        private readonly List<IJsonNode> _children;
        public IReadOnlyList<IJsonNode> Children => _children ?? new List<IJsonNode>();

        public JsonArray(params IJsonArrayContent[] elements) : this(elements.AsEnumerable()) { }
        public JsonArray(IEnumerable<IJsonArrayContent>? elements)
        {
            if (elements is null)
            {
                _children = new List<IJsonNode>(0);
            }
            else
            {
                _children = new List<IJsonNode>();
                foreach (var property in elements) _children.Add(property);
            }
        }

        public JsonArray(ReadOnlySpan<char> text, ref int offset)
        {
            _children = new List<IJsonNode>();

            offset++;

            while (offset < text.Length)
            {
                var character = text[offset];

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
