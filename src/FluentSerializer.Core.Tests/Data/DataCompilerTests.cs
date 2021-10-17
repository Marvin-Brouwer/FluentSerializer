using FluentAssertions;
using FluentSerializer.Core.Data;
using FluentSerializer.Core.Data.Json;
using FluentSerializer.Core.Data.Xml;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FluentSerializer.Core.Tests.Data
{
    public sealed class DataCompilerTests
    {
        private readonly JsonObject _jsonTest;
        private readonly string _expectedJsonFormatted;
        private readonly string _expectedJsonSlim;
        private readonly XmlElement _xmlTest;
        private readonly string _expectedXmlFormatted;
        private readonly string _expectedXmlSlim;

        public DataCompilerTests()
        {
            _jsonTest = new JsonObject(
                new JsonProperty("prop", JsonValue.String("Test")),
                new JsonProperty("prop2", new JsonObject(
                    new JsonProperty("array", new JsonArray(
                        new JsonObject(),
                        new JsonArray()
                    )),
                    new JsonProperty("prop3", new JsonValue("1")),
                    new JsonProperty("prop4", new JsonValue("true")),
                    new JsonProperty("prop5", new JsonValue("null"))
                ))
            );

            _expectedJsonFormatted = "{\r\n\t\"prop\" : \"Test\",\r\n\t\"prop2\" : {\r\n\t\t\"array\" : " +
                "[\r\n\t\t\t{\r\n\t\t\t},\r\n\t\t\t[\r\n\t\t\t]\r\n\t\t],\r\n\t\t\"prop3\" : 1,\r\n\t\t\"prop4\" : true,\r\n\t\t\"prop5\" : null\r\n\t}\r\n}";
            _expectedJsonSlim = "{\"prop\":\"Test\",\"prop2\":{\"array\":[{},[]],\"prop3\":1,\"prop4\":true,\"prop5\":null}}";

            _xmlTest = new XmlElement("Class",
                new XmlAttribute("someAttribute", "1"),
                new XmlElement("someProperty", new XmlElement("AnotherClass")),
                new XmlText("text here")
            );

            _expectedXmlFormatted = "<Class\r\n\t someAttribute=\"1\">\r\n\t<someProperty>\r\n\t\t<AnotherClass />\r\n\t</someProperty>\r\n\ttext here\r\n</Class>";
            _expectedXmlSlim = "<Class someAttribute=\"1\"><someProperty><AnotherClass /></someProperty>text here</Class>";
        }

        [Theory, InlineData(true), InlineData(false)]
        public void JsonObjectToStringTests(bool format)
        {
            // Arrange
            var expected = format ? _expectedJsonFormatted : _expectedJsonSlim;

            // Act
            var result = _jsonTest.ToString(format);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Theory, InlineData(true), InlineData(false)]
        public void JsonWriterTests(bool format)
        {
            // Arrange
            var expected = format ? _expectedJsonFormatted : _expectedJsonSlim;
            var serialJsonWriter = new SerialJsonWriter(format, true);

            // Act
            var result = serialJsonWriter.Write(_jsonTest);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Theory, InlineData(true), InlineData(false)]
        public void XmlElementToStringTests(bool format)
        {
            // Arrange
            var expected = format ? _expectedXmlFormatted : _expectedXmlSlim;

            // Act
            var result = _xmlTest.ToString(format);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Theory, InlineData(true), InlineData(false)]
        public void XmlWriterTests(bool format)
        {
            // Arrange
            var expected = format ? _expectedXmlFormatted : _expectedXmlSlim;
            var serialXmlWriter = new SerialXmlWriter(format, true);

            // Act
            var result = serialXmlWriter.Write(_xmlTest);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
