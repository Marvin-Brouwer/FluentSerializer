using FluentSerializer.Core.DataNodes;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FluentSerializer.Json.DataNodes
{
    [DebuggerDisplay("{ }")]
    public readonly struct JsonObject : IJsonContainer
    {
        private readonly List<IJsonNode> _children;
        public IReadOnlyList<IJsonNode> Children => _children ?? new List<IJsonNode>();

        public string Name { get; }

        public JsonObject(IEnumerable<JsonProperty> properties)
        {

            const string className = "{ }";
            Name = className;

            _children = new List<IJsonNode>();
            foreach (var property in properties) _children.Add(property);
        }

        public JsonObject(params JsonProperty[] properties) : this(properties.AsEnumerable()) { }

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
    }
}
