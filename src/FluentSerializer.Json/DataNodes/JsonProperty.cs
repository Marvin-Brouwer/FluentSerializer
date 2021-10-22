using Ardalis.GuardClauses;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace FluentSerializer.Json.DataNodes
{
    [DebuggerDisplay("{Name,nq}: /* */,")]
    public readonly struct JsonProperty : IJsonContainer, IEquatable<IJsonNode>
    {
        public string Name { get; }

        private readonly IJsonNode[] _children;
        public IReadOnlyList<IJsonNode> Children => _children ?? new IJsonNode[] { };

        private JsonProperty(string name, IJsonNode? value = null)
        {
            Guard.Against.InvalidName(name, nameof(name));

            Name = name;
            _children = value is null ? new IJsonNode[0] : new IJsonNode[1] { value }; ;
        }
        public JsonProperty(string name, JsonValue? value = null) : this(name, (IJsonNode?)value) { }
        public JsonProperty(string name, JsonObject? value = null) : this(name, (IJsonNode?)value) { }
        public JsonProperty(string name, JsonArray? value = null) : this(name, (IJsonNode?)value) { }

        public JsonProperty(ReadOnlySpan<char> text, StringBuilder stringBuilder, ref int offset)
        {
            const char nameEndCharacter = '"';
            const char propertyAssignmentCharacter = ':';
            const char propertyValueEndCharacter = ',';
            const char objectStartCharacter = '{';
            const char objectEndCharacter = '}';
            const char arrayStartCharacter = '[';
            const char arrayEndCharacter = ']';

            stringBuilder.Clear();
            while (offset < text.Length)
            {
                var character = text[offset];

                if (character == objectEndCharacter) break;
                if (character == arrayEndCharacter) break;
                offset++;
                if (character == propertyValueEndCharacter) break;
                if (character == nameEndCharacter) break;

                stringBuilder.Append(character);
            }

            Name = stringBuilder.ToString();
            stringBuilder.Clear();

            while (offset < text.Length)
            {
                var character = text[offset];

                if (character == objectEndCharacter) break;
                if (character == arrayEndCharacter) break;
                offset++;
                if (character == propertyValueEndCharacter) break;
                if (char.IsWhiteSpace(character)) continue;

                if (character == propertyAssignmentCharacter) break;
            }

            _children = new IJsonNode[1];

            while (offset < text.Length)
            {
                var character = text[offset];

                if (character == objectEndCharacter) return;
                if (character == arrayEndCharacter) return;

                if (character == objectStartCharacter)
                {
                    _children[0] = new JsonObject(text, stringBuilder, ref offset);
                    return;
                }
                if (character == arrayStartCharacter)
                {
                    _children[0] = new JsonArray(text, stringBuilder, ref offset);
                    return;
                }

                if (!char.IsWhiteSpace(character)) break;
                offset++;
                if (character == propertyValueEndCharacter) return;
            }

            _children[0] = new JsonValue(text, stringBuilder, ref offset);
        }

        public override string ToString() => ToString(false);
        public string ToString(bool format) => WriteTo(new StringBuilder(), format).ToString();
        public StringBuilder WriteTo(StringBuilder stringBuilder, bool format = true, int indent = 0, bool writeNull = true)
        {
            const char wrappingCharacter = '"';
            const char assignmentCharacter = ':';
            const char spacer = ' ';

            var childValue = Children.FirstOrDefault();
            if (!writeNull && childValue is null) return stringBuilder;

            stringBuilder
                .Append(wrappingCharacter)
                .Append(Name)
                .Append(wrappingCharacter);

            if (format) stringBuilder.Append(spacer);
            stringBuilder.Append(assignmentCharacter);
            if (format) stringBuilder.Append(spacer);

            if (childValue is null) stringBuilder.Append("null");
            else stringBuilder.AppendNode(childValue, format, indent);

            return stringBuilder;
        }
        public override bool Equals(object? obj)
        {
            if (obj is not IJsonNode node) return false;
            return Equals(node);
        }
        public bool Equals([AllowNull] IJsonNode other)
        {
            if (other is not JsonProperty otherProperty) return false;

            if (Name != otherProperty.Name) return false;

            return Children[0].Equals(otherProperty.Children[0]);
        }
    }
}
