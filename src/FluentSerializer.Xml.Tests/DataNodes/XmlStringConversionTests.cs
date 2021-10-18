using FluentAssertions;
using FluentSerializer.Xml.DataNodes;
using Xunit;

namespace FluentSerializer.Xml.Tests.DataNodes
{
    public sealed class XmlStringConversionTests
    {
        private readonly XmlElement _testObject;
        private readonly string _testXmlFormatted;
        private readonly string _testXmlSlim;

        public XmlStringConversionTests()
        {
            _testObject = new XmlElement("Class",
                new XmlAttribute("someAttribute", "1"),
                new XmlElement("someProperty", new XmlElement("AnotherClass")),
                new XmlText("text here")
            );

            _testXmlFormatted = "<Class\r\n someAttribute=\"1\">\r\n\t<someProperty>\r\n\t\t<AnotherClass />\r\n\t</someProperty>\r\n\ttext here\r\n</Class>";           
            _testXmlSlim = "<Class someAttribute=\"1\"><someProperty><AnotherClass /></someProperty>text here</Class>";
        }

        [Theory, InlineData(true), InlineData(false)]
        public void XmlElementToStringTests(bool format)
        {
            // Arrange
            var expected = format ? _testXmlFormatted : _testXmlSlim;

            // Act
            var result = _testObject.ToString(format);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
