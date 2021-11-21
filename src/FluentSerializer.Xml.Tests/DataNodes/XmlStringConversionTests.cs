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

            _testXmlFormatted = "<Class\n someAttribute=\"1\">\n\t<!-- Comment -->\n\t" +
                "<![CDATA[<p>some xml data here</p>]]>\n\t" + 
                "<someProperty>\n\t\t<AnotherClass />\n\t</someProperty>\n\ttext here\n</Class>";           
            _testXmlSlim = "<Class someAttribute=\"1\"><!-- Comment --><![CDATA[<p>some xml data here</p>]]>" +
                "<someProperty><AnotherClass /></someProperty>text here</Class>";
        }

        [Theory,
            Trait("Category", "UnitTest"), Trait("DataFormat", "XML"), 
            InlineData(true), InlineData(false)]
        public void XmlElementToString(bool format)
        {
            // Arrange
            var expected = format ? _testXmlFormatted : _testXmlSlim;

            // Act
            var result = _testObject.WriteTo(StringFastPool, format).FixNewLine();

            // Assert
            result.ShouldBeBinaryEquatableTo(expected);
        }

        [Theory,
            Trait("Category", "UnitTest"), Trait("DataFormat", "XML"), 
            InlineData(true), InlineData(false)]
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
