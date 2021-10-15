using Ardalis.GuardClauses;
using FluentSerializer.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentSerializer.Core.Data
{
    public interface IDataContainer<out TValue> : IDataValue
        where TValue : IDataValue
    {
        new IReadOnlyList<TValue> Value { get; }
    }
    public interface IDataValue
    {
        public string Name { get; }
        public string? Value { get; }
    }
    public interface IJsonContainer : IDataContainer<IJsonNode>, IJsonNode { }
    public interface IJsonNode : IDataValue { }

    public sealed class JsonObject : IJsonContainer
    {
        public IReadOnlyList<IJsonNode> Value {get;}

        string? IDataValue.Value => throw new NotImplementedException();

        public string Name { get; }

        private JsonObject(List<JsonProperty> properties)
        {
            const string className = "{ }";
            Name = className;
            Value = properties;
        }
        public JsonObject() : this(new List<JsonProperty>(0)) { }
        public JsonObject(IEnumerable<JsonProperty> properties) : this(properties.ToList()) { }
        public JsonObject(params JsonProperty[] properties) : this(properties.AsEnumerable()) { }
    }
    public sealed class JsonArray : IJsonContainer
    {
        public IReadOnlyList<IJsonNode> Value { get; }

        string? IDataValue.Value => throw new NotImplementedException();

        public string Name { get; }

        public JsonArray(IEnumerable<IJsonContainer> elements)
        {
            const string arrayName = "[ ]";
            Name = arrayName;
            Value = elements is null ? new List<IJsonNode>(0) : new(elements);
        }
        public JsonArray(params IJsonContainer[] elements) : this(elements.AsEnumerable()) {  }
        public JsonArray() : this(new List<IJsonContainer>(0)) { }
    }
    public sealed class JsonProperty : IJsonContainer
    {
        public string Name { get; }

        public IReadOnlyList<IJsonNode> Value { get; }

        string? IDataValue.Value => throw new NotImplementedException();

        private JsonProperty(string name, IJsonNode? value = null)
        {
            Guard.Against.InvalidName(name, nameof(name));

            Name = name;
            var valueList = value is null ? new List<IJsonNode>(0) : new(1) { value };
            Value = valueList.AsReadOnly();
        }
        public JsonProperty(string name, string? value = null) : this(name, new JsonValue(value)) { }
        public JsonProperty(string name, IJsonContainer? value = null) : this(name, (IJsonNode?)value) { }
    }
    public sealed class JsonValue : IJsonNode
    {
        public string Name { get; }
        public string? Value { get; }

        public JsonValue(string? value = null)
        {
            const string valueName = "#value";
            Name = valueName;
            Value = value;
        }
    }

    public interface IXmlContainer : IDataContainer<IXmlNode>, IXmlNode { }
    public interface IXmlNode : IDataValue { }
    public sealed class XmlElement : IXmlContainer
    {
        private readonly List<XmlAttribute> _attributes;
        private readonly List<XmlElement> _children;
        private readonly List<XmlText> _textNodes;


        public IReadOnlyList<IXmlNode> Value
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

        string? IDataValue.Value => throw new NotImplementedException();
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
    }
    public sealed class XmlText : IXmlNode
    {
        public string Name { get; }
        public string? Value { get; }

        public XmlText(string value = null)
        {
            const string textName = "#text";
            Name = textName;
            Value = value;
        }


        public class Test
        {
            public Test()
            {
                var jsonTest = new JsonObject(
                    new JsonProperty("prop", "Test"),
                    new JsonProperty("prop2", new JsonObject(
                        new JsonProperty("array", new JsonArray(
                            new JsonObject()
                        ))
                    ))
                );

                var xmlTest = new XmlElement("Class",
                    new XmlAttribute("someAttribute", "1"),
                    new XmlElement("someProperty", new XmlElement("AnotherClass")),
                    new XmlText("text here")
                );
            }
        }
    }
}
