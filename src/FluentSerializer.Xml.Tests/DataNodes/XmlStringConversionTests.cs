using FluentAssertions;
using FluentSerializer.Core.Dirty;
using FluentSerializer.Core.Tests.Extensions;
using FluentSerializer.Xml.DataNodes;
using Microsoft.Extensions.ObjectPool;
using System;
using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.DataNodes
{
    public sealed class XmlStringConversionTests
    {
        private static readonly ObjectPoolProvider ObjectPoolProvider = new DefaultObjectPoolProvider();
        public static readonly ObjectPool<StringFast> StringFastPool = ObjectPoolProvider.CreateStringFastPool();

        private readonly IXmlElement _testObject;
        private readonly string _testXmlFormatted;
        private readonly string _testXmlSlim;

        public XmlStringConversionTests()
        {
            _testObject = Element("Class",
                Attribute("someAttribute", "1"),
                Comment("Comment"),
                CData("<p>some xml data here</p>"),
                Element("someProperty", Element("AnotherClass")),
                Text("text here")
            );

            _testXmlFormatted = "<Class\r\n someAttribute=\"1\">\r\n\t<!-- Comment -->\r\n\t" +
                "<![CDATA[<p>some xml data here</p>]]>\r\n\t" + 
                "<someProperty>\r\n\t\t<AnotherClass />\r\n\t</someProperty>\r\n\ttext here\r\n</Class>";           
            _testXmlSlim = "<Class someAttribute=\"1\"><!-- Comment --><![CDATA[<p>some xml data here</p>]]>" +
                "<someProperty><AnotherClass /></someProperty>text here</Class>";
        }

        [Theory, InlineData(true), InlineData(false)]
        public void XmlElementToString(bool format)
        {
            // Arrange
            var expected = format ? _testXmlFormatted : _testXmlSlim;

            // Act
            var result = _testObject.WriteTo(StringFastPool, format);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Theory, InlineData(true), InlineData(false)]
        public void StringToObject(bool format)
        {
            // Arrange
            var expected = _testObject;
            var input = format ? _testXmlFormatted : _testXmlSlim;

            // Act
            var result = XmlParser.Parse(input.AsSpan());

            // Assert
            result.Should().BeEquatableTo(expected);
        }
    }
}
