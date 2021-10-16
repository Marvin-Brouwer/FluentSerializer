using Ardalis.GuardClauses;
using FluentSerializer.Core.Extensions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FluentSerializer.Core.Data
{
    public interface IDataContainer<out TValue> : IDataNode
        where TValue : IDataNode
    {
        IReadOnlyList<TValue> Children { get; }
    }
    public interface IDataValue : IDataNode
    {
        string? Value { get; }
    }
    public interface IDataNode
    {
        string Name { get; }

        string ToString(bool format);
        StringBuilder WriteTo(StringBuilder stringBuilder, bool format = true, int indent = 0, bool writeNull = true);

    }
    public interface IJsonContainer : IDataContainer<IJsonNode>, IJsonNode { }
    public interface IJsonValue : IDataValue, IJsonNode { }
    public interface IJsonNode : IDataNode { }

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

    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendNode(this StringBuilder stringBuilder, IDataNode node, bool format, int indent)
        {
            return node.WriteTo(stringBuilder, format, format ? indent : 0);
        }
        public static StringBuilder AppendOptionalNewline(this StringBuilder stringBuilder, bool newLine)
        {
            if (!newLine) return stringBuilder;

            return stringBuilder.AppendLine();
        }
        public static StringBuilder AppendOptionalIndent(this StringBuilder stringBuilder, int indent, bool format)
        {
            const char indentChar = '\t';

            if (!format) return stringBuilder;
            return stringBuilder.Append(indentChar, indent);
        }
    }
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

    [DebuggerDisplay(nameof(ToString))]
    public sealed class JsonProperty : IJsonContainer
    {
        public string Name { get; }

        public IReadOnlyList<IJsonNode> Children { get; }

        private JsonProperty(string name, IJsonNode? value = null)
        {
            Guard.Against.InvalidName(name, nameof(name));

            Name = name;
            var valueList = value is null ? new List<IJsonNode>(0) : new(1) { value };
            Children = valueList.AsReadOnly();
        }
        public JsonProperty(string name, JsonValue? value = null) : this(name, (IJsonNode?)value) { }
        public JsonProperty(string name, JsonObject? value = null) : this(name, (IJsonNode?)value) { }
        public JsonProperty(string name, JsonArray? value = null) : this(name, (IJsonNode?)value) { }

        public override string ToString() => ToString(true);
        public string ToString(bool format = true) => WriteTo(new StringBuilder(), format).ToString();
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
            stringBuilder .Append(assignmentCharacter);
            if (format) stringBuilder.Append(spacer);

            if (childValue is null) stringBuilder.Append("null");
            else stringBuilder.AppendNode(childValue, format, indent);

            return stringBuilder;
        }
    }

    [DebuggerDisplay(nameof(Value))]
    public sealed class JsonValue : IJsonNode
    {
        public string Name { get; }
        public string? Value { get; }

        public static JsonValue String(string? value = null) => new ($"\"{value}\"");


        public JsonValue(string? value = null)
        {
            const string valueName = "#value";
            Name = valueName;
            Value = value;
        }

        public override string ToString() => ToString(true);
        public string ToString(bool format = true) => WriteTo(new StringBuilder(), format).ToString();
        public StringBuilder WriteTo(StringBuilder stringBuilder, bool format = true, int indent = 0, bool writeNull = true)
        {
            // JSON does not support empty property assignment or array members
            if (!writeNull && string.IsNullOrEmpty(Value)) return stringBuilder;

           return stringBuilder.Append(Value);
        }
    }

    public interface IXmlContainer : IDataContainer<IXmlNode>, IXmlNode { }
    public interface IXmlValue : IDataValue, IXmlNode { }
    public interface IXmlNode : IDataNode { }
    public sealed class XmlElement : IXmlContainer
    {
        private readonly List<XmlAttribute> _attributes;
        private readonly List<XmlElement> _children;
        private readonly List<XmlText> _textNodes;


        public IReadOnlyList<IXmlNode> Children
        {
            get
            {
                var value = new List<IXmlNode>();
                value.AddRange(_attributes);
                value.AddRange(_children);
                value.AddRange(_textNodes);

                return value.AsReadOnly();
            }
        }

        public string Name { get; }


        public XmlElement(string name, IEnumerable<IXmlNode> childNodes)
        {
            Guard.Against.InvalidName(name, nameof(name));

            Name = name;
            _attributes = new();
            _children = new();
            _textNodes = new();

            _attributes.AddRange(childNodes.Where(node => node is XmlAttribute).Cast<XmlAttribute>());
            _children.AddRange(childNodes.Where(node => node is XmlElement).Cast<XmlElement>());
            _textNodes.AddRange(childNodes.Where(node => node is XmlText).Cast<XmlText>());
        }
        public XmlElement(string name) : this(name, new List<IXmlNode>(0)) { }
        public XmlElement(string name, params IXmlNode[] childNodes) : this(name, childNodes.AsEnumerable()) { }

        public override string ToString() => ToString(true);
        public string ToString(bool format = true) => WriteTo(new StringBuilder(), format).ToString();
        public StringBuilder WriteTo(StringBuilder stringBuilder, bool format = true, int indent = 0, bool writeNull = true)
        {
            const char tagStartCharacter = '<';
            const char tagTerminaterCharacter = '/';
            const char tagEndCharacter = '>';
            const char spacer = ' ';

            var children = Children;
            var childIndent = format ? indent + 1 : 0;

            if (!writeNull && !children.Any()) return stringBuilder;

            stringBuilder
                .AppendOptionalNewline(false)
                .Append(tagStartCharacter)
                .Append(Name);

            if (!children.Any()) return stringBuilder
                    .Append(spacer)
                    .Append(tagTerminaterCharacter)
                    .Append(tagEndCharacter);

            foreach (var attribute in _attributes)
            {
                stringBuilder
                    .AppendOptionalNewline(format)
                    .AppendOptionalIndent(childIndent, format)
                    .Append(spacer)
                    .AppendNode(attribute, format, childIndent);
            }
            stringBuilder
                .Append(tagEndCharacter);

            foreach (var child in _children)
            {
                stringBuilder
                    .AppendOptionalNewline(format)
                    .AppendOptionalIndent(childIndent, format)
                    .AppendNode(child, format, childIndent);
            }
            foreach (var text in _textNodes)
            {
                if (text == _textNodes[0])
                {
                    stringBuilder
                        .AppendOptionalNewline(format)
                        .AppendOptionalIndent(childIndent, format);
                }
                stringBuilder
                    .AppendNode(text, text == _textNodes[0], childIndent);
            }

            stringBuilder
                .AppendOptionalNewline(format)
                .AppendOptionalIndent(indent, format)
                .Append(tagStartCharacter)
                .Append(tagTerminaterCharacter)
                .Append(Name)
                .Append(tagEndCharacter);

            return stringBuilder;
        }
    }
    public sealed class XmlAttribute : IXmlNode
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
        public string ToString(bool format = true) => WriteTo(new StringBuilder(), format).ToString();
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
    public sealed class XmlText : IXmlNode
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
