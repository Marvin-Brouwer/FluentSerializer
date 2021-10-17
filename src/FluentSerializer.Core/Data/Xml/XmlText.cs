using System.Text;

namespace FluentSerializer.Core.Data.Xml
{
    public readonly struct XmlText : IXmlNode
    {
        public string Name { get; }
        public string? Value { get; }

        public XmlText(string? value = null)
        {
            const string textName = "#text";
            Name = textName;
            Value = value;
        }

        public override string ToString() => ToString(true);
        public string ToString(bool format = true) => WriteTo(new StringBuilder(), format).ToString();
        public StringBuilder WriteTo(StringBuilder stringBuilder, bool format = true, int indent = 0, bool writeNull = true)
        {
            if (!writeNull && Value is null) return stringBuilder;

            stringBuilder.Append(Value);

            return stringBuilder;
        }
    }
}
