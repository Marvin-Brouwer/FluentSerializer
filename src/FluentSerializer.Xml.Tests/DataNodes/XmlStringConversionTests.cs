using FluentAssertions;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.Tests.Extensions;
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
            using var stream = new MemoryStream();
            using var writer = new StreamWriter(stream);

            _testObject.WriteTo(StringBuilderPool, writer, format);
            writer.Flush();
            var result = Encoding.UTF8.GetString(stream.ToArray());

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
            result.Should().BeEquatableTo<IXmlNode>(expected);
        }
    }
}
