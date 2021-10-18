using System.Diagnostics;
using Ardalis.GuardClauses;
using FluentSerializer.Core.Extensions;
using System.Text;

namespace FluentSerializer.Core.Data.Xml
{
    [DebuggerDisplay("{Name,nq}={Value}")]
    public readonly struct XmlAttribute : IXmlNode
    {
        public string Name { get; }
        public string? Value { get; }

        public XmlAttribute(string name, string? value = null)
        {
            Guard.Against.InvalidName(name, nameof(name));

            Name = name;
            Value = value;
        }

        public override string ToString() => ToString(true);
        public string ToString(bool format) => WriteTo(new StringBuilder(), format).ToString();
        public StringBuilder WriteTo(StringBuilder stringBuilder, bool format = true, int indent = 0, bool writeNull = true)
        {
            const char wrappingCharacter = '"';
            const char assignmentCharacter = '=';

            if (!writeNull && Value is null) return stringBuilder;

            stringBuilder
                .Append(Name)
                .Append(assignmentCharacter)
                .Append(wrappingCharacter)
                .Append(Value)
                .Append(wrappingCharacter);

            return stringBuilder;
        }
    }
}
