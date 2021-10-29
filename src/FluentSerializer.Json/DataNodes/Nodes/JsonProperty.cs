using Ardalis.GuardClauses;
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
    [DebuggerDisplay("{Name,nq}: {GetDebugValue(), nq},")]
    public readonly struct JsonProperty : IJsonProperty
    {
        [DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
        private string GetDebugValue()
        {
            if (_children.Length == 0) return JsonConstants.NullValue;
            var value = _children[0];
            if (value is JsonValue jsonValue) return jsonValue.Value ?? JsonConstants.NullValue;
            return value.Name;
        }

        public string Name { get; }

        private readonly IJsonNode[] _children;
        public IReadOnlyList<IJsonNode> Children => _children;

        private JsonProperty(string name, IJsonNode? value = null)
        {
            Guard.Against.InvalidName(name, nameof(name));

            Name = name;
            _children = value is null ? new IJsonNode[0] : new IJsonNode[1] { value }; ;
        }

        public JsonProperty(string name, IJsonValue? value = null) : this(name, (IJsonNode?)value) { }
        public JsonProperty(string name, IJsonObject? value = null) : this(name, (IJsonNode?)value) { }
        public JsonProperty(string name, IJsonArray? value = null) : this(name, (IJsonNode?)value) { }

        public JsonProperty(ReadOnlySpan<char> text, ref int offset)
        {
            var stringBuilder = new StringBuilder(128);
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
                    _children[0] = new JsonObject(text, ref offset);
                    return;
                }
                if (character == JsonConstants.ArrayStartCharacter)
                {
                    _children[0] = new JsonArray(text, ref offset);
                    return;
                }

                if (!char.IsWhiteSpace(character)) break;
                offset++;
                if (character == JsonConstants.DividerCharacter) return;
            }

            _children[0] = new JsonValue(text, ref offset);
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder = AppendTo(stringBuilder);
            return stringBuilder.ToString();
        }

        public void WriteTo(ObjectPool<StringBuilder> stringBuilders, TextWriter writer, bool format = true, int indent = 0, bool writeNull = true)
        {
            Guard.Against.NullOrWhiteSpace(Name, nameof(Name), "The property was is an illegal state, it contains no Name");

            var stringBuilder = stringBuilders.Get();

            stringBuilder = AppendTo(stringBuilder, format, indent, writeNull);
            writer.Write(stringBuilder);

            stringBuilder.Clear();
            stringBuilders.Return(stringBuilder);
        }

        public StringBuilder AppendTo(StringBuilder stringBuilder, bool format = true, int indent = 0, bool writeNull = true)
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
            else stringBuilder.AppendNode(childValue, format, indent, writeNull);

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
