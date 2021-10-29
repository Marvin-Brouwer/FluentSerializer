using FluentAssertions;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.Tests.Extensions;
using Microsoft.Extensions.ObjectPool;
using System;
using System.IO;
using System.Text;
using Xunit;
using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.DataNodes
{
    public sealed class JsonStringConversionTests
    {
        private static readonly ObjectPoolProvider ObjectPoolProvider = new DefaultObjectPoolProvider();
        public static readonly ObjectPool<StringBuilder> StringBuilderPool = ObjectPoolProvider.CreateStringBuilderPool();

        private readonly IJsonObject _testObject;
        private readonly string _testJsonFormatted;
        private readonly string _testJsonSlim;

        public JsonStringConversionTests
()
        {
            _testObject = Object(
                Property("prop", Value($"\"Test\"")),
                Property("prop2", Object(
                    Property("array", Array(
                        Object(),
                        Array()
                    )),
                    Property("prop3", Value("1")),
                    Property("prop4", Value("true")),
                    Property("prop5", Value("null"))
                ))
            );

            _testJsonFormatted = "{\r\n\t\"prop\" : \"Test\",\r\n\t\"prop2\" : {\r\n\t\t\"array\" : " +
                "[\r\n\t\t\t{\r\n\t\t\t},\r\n\t\t\t[\r\n\t\t\t]\r\n\t\t],\r\n\t\t\"prop3\" : 1,\r\n\t\t\"prop4\" : true,\r\n\t\t\"prop5\" : null\r\n\t}\r\n}";
            _testJsonSlim = "{\"prop\":\"Test\",\"prop2\":{\"array\":[{},[]],\"prop3\":1,\"prop4\":true,\"prop5\":null}}";

        }
        
        [Theory, InlineData(true), InlineData(false)]
        public void JsonObjectToString(bool format)
        {
            // Arrange
            var expected = format ? _testJsonFormatted : _testJsonSlim;

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
            var input = format ? _testJsonFormatted : _testJsonSlim;

            // Act
            var result = JsonParser.Parse(input.AsSpan());

            // Assert
            result.Should().BeEquatableTo<IJsonNode>(expected);
        }
    }
}
