using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FluentSerializer.Core.Data.Json
{
    [DebuggerDisplay(nameof(ToString))]
    public sealed class JsonObject : IJsonContainer
    {
        public IReadOnlyList<IJsonNode> Children {get;}

        public string Name { get; }

        private JsonObject(List<JsonProperty> properties)
        {
            const string className = "{ }";
            Name = className;
            Children = properties;
        }
        public JsonObject() : this(new List<JsonProperty>(0)) { }
        public JsonObject(IEnumerable<JsonProperty> properties) : this(properties.ToList()) { }
        public JsonObject(params JsonProperty[] properties) : this(properties.AsEnumerable()) { }

        public override string ToString() => ToString(true);
        public string ToString(bool format = true) => WriteTo(new StringBuilder(), format).ToString();
        public StringBuilder WriteTo(StringBuilder stringBuilder, bool format = true, int indent = 0, bool writeNull = true)
        {
            const char openingCharacter = '{';
            const char separatorCharacter = ',';
            const char closingCharacter = '}';

            var childIndent = indent + 1;

            stringBuilder
                .Append(openingCharacter);
            foreach (var child in Children)
            {
                stringBuilder
                    .AppendOptionalNewline(format)
                    .AppendOptionalIndent(childIndent, format)
                    .AppendNode(child, format, childIndent);
                if (child != Children[^1]) 
                    stringBuilder.Append(separatorCharacter);
            }
            stringBuilder
                .AppendOptionalNewline(format)
                .AppendOptionalIndent(indent, format)
                .Append(closingCharacter);

            return stringBuilder;
        }
    }
}
