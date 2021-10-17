using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FluentSerializer.Core.Data.Json
{
    [DebuggerDisplay(nameof(ToString))]
    public sealed class JsonArray : IJsonContainer
    {
        public IReadOnlyList<IJsonNode> Children { get; }

        public string Name { get; }

        public JsonArray(IEnumerable<IJsonContainer> elements)
        {
            const string arrayName = "[ ]";
            Name = arrayName;
            Children = elements is null ? new List<IJsonNode>(0) : new(elements);
        }
        public JsonArray(params IJsonContainer[] elements) : this(elements.AsEnumerable()) {  }
        public JsonArray() : this(new List<IJsonContainer>(0)) { }

        public override string ToString() => ToString(true);
        public string ToString(bool format = true) => WriteTo(new StringBuilder(), format).ToString();
        public StringBuilder WriteTo(StringBuilder stringBuilder, bool format = true, int indent = 0, bool writeNull = true)
        {
            const char openingCharacter = '[';
            const char separatorCharacter = ',';
            const char closingCharacter = ']';

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
