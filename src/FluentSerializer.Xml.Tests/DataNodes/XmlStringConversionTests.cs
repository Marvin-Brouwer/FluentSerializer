using FluentAssertions;
using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Xml.DataNodes;
using Microsoft.Extensions.ObjectPool;
using System;
using System.IO;
using System.Text;
using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.DataNodes
{
    public sealed class XmlStringConversionTests
    {
        private static readonly ObjectPoolProvider ObjectPoolProvider = new DefaultObjectPoolProvider();
        public static readonly ObjectPool<StringBuilder> StringBuilderPool = ObjectPoolProvider.CreateStringBuilderPool();

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
            using var stream = new MemoryStream();
            using var writer = new StreamWriter(stream);

            _testObject.WriteTo(StringBuilderPool, writer, format);
            writer.Flush();
            var result = Encoding.UTF8.GetString(stream.ToArray()).FixNewLine();

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
