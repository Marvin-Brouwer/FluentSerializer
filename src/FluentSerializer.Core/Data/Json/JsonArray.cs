using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FluentSerializer.Core.Data.Json
{
    [DebuggerDisplay("[ ]")]
    public sealed class JsonArray : IJsonContainer
    {
        public IReadOnlyList<IJsonNode> Children { get; }

        public string Name { get; }

        public JsonArray(IEnumerable<IJsonContainer> elements)
        {
            const string arrayName = "[ ]";
            Name = arrayName;
            Children = elements.ToList();
        }
        public JsonArray(params IJsonContainer[] elements) : this(elements.AsEnumerable()) {  }

        public override string ToString() => ToString(true);
        public string ToString(bool format) => WriteTo(new StringBuilder(), format).ToString();
        public StringBuilder WriteTo(StringBuilder stringBuilder, bool format = true, int indent = 0, bool writeNull = true)
        {
            const char openingCharacter = '[';
            const char separatorCharacter = ',';
            const char closingCharacter = ']';

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

                if (i != Children.Count - 1) stringBuilder.Append(separatorCharacter);
            }

            stringBuilder
                .AppendOptionalNewline(format)
                .AppendOptionalIndent(indent, format)
                .Append(closingCharacter);

            return stringBuilder;
        }
    }
}
