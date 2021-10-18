using Ardalis.GuardClauses;
using FluentSerializer.Core.Extensions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FluentSerializer.Core.Data.Json
{
    [DebuggerDisplay("{Name,nq}: /* */,")]
    public readonly struct JsonProperty : IJsonContainer
    {
        public string Name { get; }

        public IReadOnlyList<IJsonNode> Children { get; }

        private JsonProperty(string name, IJsonNode? value = null)
        {
            Guard.Against.InvalidName(name, nameof(name));

            Name = name;
            Children = value is null ? new IJsonNode[0] : new IJsonNode[1] { value }; ;
        }
        public JsonProperty(string name, JsonValue? value = null) : this(name, (IJsonNode?)value) { }
        public JsonProperty(string name, JsonObject? value = null) : this(name, (IJsonNode?)value) { }
        public JsonProperty(string name, JsonArray? value = null) : this(name, (IJsonNode?)value) { }

        public override string ToString() => ToString(true);
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
    }
}
