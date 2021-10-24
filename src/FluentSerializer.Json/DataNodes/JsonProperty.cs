using Ardalis.GuardClauses;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FluentSerializer.Json.DataNodes
{
    [DebuggerDisplay("{Name,nq}: {GetDebugValue(), nq},")]
    public readonly struct JsonProperty: IJsonContainer
    {
        [DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
        private string GetDebugValue()
        {
            if (_children.Length == 0) return JsonConstants.NullValue;
            var value = _children[0];
            if (value is JsonValue jsonValue) return jsonValue.Value ?? JsonConstants.NullValue;
            return value.Name;
        }

        public string Name { get; init; }

        private readonly IJsonNode[] _children;
        public IReadOnlyList<IJsonNode> Children => _children;

        private static string CheckName(string name, string propertyName)
        {
            Guard.Against.InvalidName(name, propertyName);
            return name;
        }

        private JsonProperty(string name, IJsonNode? value = null)
        {
            Name = CheckName(name, nameof(name));
            _children = value is null ? new IJsonNode[0] : new IJsonNode[1] { value }; ;
        }

        public JsonProperty(string name, JsonValue? value = null) : this(name, (IJsonNode?)value) { }
        public JsonProperty(string name, JsonObject? value = null) : this(name, (IJsonNode?)value) { }
        public JsonProperty(string name, JsonArray? value = null) : this(name, (IJsonNode?)value) { }

        public JsonProperty(ReadOnlySpan<char> text, StringBuilder stringBuilder, ref int offset)
        {
            stringBuilder.Clear();
            while (offset < text.Length)
            {
                var character = text[offset];

                if (character == JsonConstants.ObjectEndCharacter) break;
                if (character == JsonConstants.ArrayEndCharacter) break;
                offset++;
                if (character == JsonConstants.DividerCharacter) break;
                if (character == JsonConstants.PropertyWrapCharacter) break;

                stringBuilder.Append(character);
            }

            Name = stringBuilder.ToString();
            stringBuilder.Clear();

            while (offset < text.Length)
            {
                var character = text[offset];

                if (character == JsonConstants.ObjectEndCharacter) break;
                if (character == JsonConstants.ArrayEndCharacter) break;
                offset++;
                if (character == JsonConstants.DividerCharacter) break;
                if (char.IsWhiteSpace(character)) continue;

                if (character == JsonConstants.PropertyAssignmentCharacter) break;
            }

            _children = new IJsonNode[1];

            while (offset < text.Length)
            {
                var character = text[offset];

                if (character == JsonConstants.ObjectEndCharacter) return;
                if (character == JsonConstants.ArrayEndCharacter) return;

                if (character == JsonConstants.ObjectStartCharacter)
                {
                    _children[0] = new JsonObject(text, stringBuilder, ref offset);
                    return;
                }
                if (character == JsonConstants.ArrayStartCharacter)
                {
                    _children[0] = new JsonArray(text, stringBuilder, ref offset);
                    return;
                }

                if (!char.IsWhiteSpace(character)) break;
                offset++;
                if (character == JsonConstants.DividerCharacter) return;
            }

            _children[0] = new JsonValue(text, stringBuilder, ref offset);
        }

        public override string ToString() => ToString(false);
        public string ToString(bool format) => WriteTo(new StringBuilder(), format).ToString();
        public StringBuilder WriteTo(StringBuilder stringBuilder, bool format = true, int indent = 0, bool writeNull = true)
        {
            Guard.Against.NullOrWhiteSpace(Name, nameof(Name), "The property was is an illegal state, it contains no Name");

            const char spacer = ' ';

            var childValue = Children.FirstOrDefault();
            if (!writeNull && childValue is null) return stringBuilder;

            stringBuilder
                .Append(JsonConstants.PropertyWrapCharacter)
                .Append(Name)
                .Append(JsonConstants.PropertyWrapCharacter);

            if (format) stringBuilder.Append(spacer);
            stringBuilder.Append(JsonConstants.PropertyAssignmentCharacter);
            if (format) stringBuilder.Append(spacer);

            if (childValue is null) stringBuilder.Append(JsonConstants.NullValue);
            else stringBuilder.AppendNode(childValue, format, indent);

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
            if (other is not JsonProperty otherProperty) return false;

            if (Name != otherProperty.Name) return false;

            return Children[0].Equals(otherProperty.Children[0]);
        }

        public override int GetHashCode()
        {
            if (_children?.Any() != true) return 0;

            var hash = new HashCode();
            hash.Add(Name.GetHashCode());
            foreach (var child in _children)
                hash.Add(child);

            return hash.ToHashCode();
        }

        #endregion
    }
}
