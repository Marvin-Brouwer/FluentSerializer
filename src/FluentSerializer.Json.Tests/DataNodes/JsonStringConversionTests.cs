using FluentAssertions;
using FluentSerializer.Core.Dirty;
using FluentSerializer.Core.Tests.Extensions;
using FluentSerializer.Json.DataNodes;
using Microsoft.Extensions.ObjectPool;
using System;
using Xunit;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.DataNodes
{
    public sealed class JsonStringConversionTests
    {
        private static readonly ObjectPoolProvider ObjectPoolProvider = new DefaultObjectPoolProvider();
        public static readonly ObjectPool<StringFast> StringFastPool = ObjectPoolProvider.CreateStringFastPool();

        private readonly IJsonObject _testObjectFormatted;
        private readonly IJsonObject _testObjectSlim;
        private readonly string _testJsonFormatted;
        private readonly string _testJsonSlim;

        public JsonStringConversionTests
()
        {
            _testObjectFormatted = Object(
                Comment("object level comment"),
                MultilineComment(
                    "object level comment\r\n" +
                    "With a new line"),
                Property("prop", Value($"\"Test\"")),
                Property("prop2", Object(
                    Property("array", Array(
                        Comment("array level comment"),
                        MultilineComment(
                            "array level comment\r\n" +
                            "With a new line"),
                        Object(),
                        Array()
                    )),
                    Property("prop3", Value("1")),
                    Property("prop4", Value("true")),
                    Property("prop5", Value("null"))
                ))
            );

            _testObjectSlim = Object(
                MultilineComment("object level comment"),
                MultilineComment(
                    "object level comment\r\n" +
                    "With a new line"),
                Property("prop", Value($"\"Test\"")),
                Property("prop2", Object(
                    Property("array", Array(
                        MultilineComment("array level comment"),
                        MultilineComment(
                            "array level comment\r\n" +
                            "With a new line"),
                        Object(),
                        Array()
                    )),
                    Property("prop3", Value("1")),
                    Property("prop4", Value("true")),
                    Property("prop5", Value("null"))
                ))
            );

            _testJsonFormatted = "{\r\n\t// object level comment\r\n\t/* object level comment\r\nWith a new line */\r\n\t\"prop\" : \"Test\",\r\n\t\"prop2\" : " +
                "{\r\n\t\t\"array\" : [\r\n\t\t\t// array level comment\r\n\t\t\t/* array level comment\r\nWith a new line */\r\n\t\t\t" +
                "{\r\n\t\t\t},\r\n\t\t\t[\r\n\t\t\t]\r\n\t\t],\r\n\t\t\"prop3\" : 1,\r\n\t\t\"prop4\" : true,\r\n\t\t\"prop5\" : null\r\n\t}\r\n}";
            _testJsonSlim = "{/* object level comment *//* object level comment\r\nWith a new line */\"prop\":\"Test\",\"prop2\":{\"array\":" +
                "[/* array level comment *//* array level comment\r\nWith a new line */{},[]],\"prop3\":1,\"prop4\":true,\"prop5\":null}}";

        }
        
        [Theory, InlineData(true), InlineData(false)]
        public void JsonObjectToString(bool format)
        {
            // Arrange
            var expected = format ? _testJsonFormatted : _testJsonSlim;
            var input = format ? _testObjectFormatted : _testObjectSlim;

            // Act
            var result = input.WriteTo(StringFastPool, format);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Theory, InlineData(true), InlineData(false)]
        public void StringToObject(bool format)
        {
            // Arrange
            var expected = format ? _testObjectFormatted : _testObjectSlim;
            var input = format ? _testJsonFormatted : _testJsonSlim;

            // Act
            var result = JsonParser.Parse(input.AsSpan());

            // Assert
            result.Should().BeEquatableTo(expected);
        }
    }
}
