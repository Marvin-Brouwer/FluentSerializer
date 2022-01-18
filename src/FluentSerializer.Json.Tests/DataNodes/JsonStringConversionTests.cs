using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Json.DataNodes;
using Xunit;

using static FluentSerializer.Json.JsonBuilder;
using FluentSerializer.Core.TestUtils.Helpers;

namespace FluentSerializer.Json.Tests.DataNodes
{
	public sealed class JsonStringConversionTests
    {
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
                    "object level comment\n" +
                    "With a new line"),
                Property("prop", Value($"\"Test\"")),
                Property("prop2", Object(
                    Property("array", Array(
                        Comment("array level comment"),
                        MultilineComment(
                            "array level comment\n" +
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
                    "object level comment\n" +
                    "With a new line"),
                Property("prop", Value($"\"Test\"")),
                Property("prop2", Object(
                    Property("array", Array(
                        MultilineComment("array level comment"),
                        MultilineComment(
                            "array level comment\n" +
                            "With a new line"),
                        Object(),
                        Array()
                    )),
                    Property("prop3", Value("1")),
                    Property("prop4", Value("true")),
                    Property("prop5", Value("null"))
                ))
            );

            _testJsonFormatted = "{\n\t// object level comment\n\t/* object level comment\nWith a new line */\n\t\"prop\": \"Test\",\n\t\"prop2\": " +
                "{\n\t\t\"array\": [\n\t\t\t// array level comment\n\t\t\t/* array level comment\nWith a new line */\n\t\t\t" +
                "{\n\t\t\t},\n\t\t\t[\n\t\t\t]\n\t\t],\n\t\t\"prop3\": 1,\n\t\t\"prop4\": true,\n\t\t\"prop5\": null\n\t}\n}";
            _testJsonSlim = "{/* object level comment *//* object level comment\nWith a new line */\"prop\":\"Test\",\"prop2\":{\"array\":" +
                "[/* array level comment *//* array level comment\nWith a new line */{},[]],\"prop3\":1,\"prop4\":true,\"prop5\":null}}";

        }
        
        [Theory,
            Trait("Category", "UnitTest"), Trait("DataFormat", "JSON"),
            InlineData(true), InlineData(false)]
        public void JsonObjectToString(bool format)
        {
            // Arrange
            var expected = format ? _testJsonFormatted : _testJsonSlim;
            var input = format ? _testObjectFormatted : _testObjectSlim;

            // Act
            var result = input.WriteTo(TestStringBuilderPool.Default, format);
            
            // Assert
            result.ShouldBeBinaryEquatableTo(expected);
        }

        [Theory,
            Trait("Category", "UnitTest"), Trait("DataFormat", "JSON"), 
            InlineData(true), InlineData(false)]
        public void StringToObject(bool format)
        {
            // Arrange
            var expected = format ? _testObjectFormatted : _testObjectSlim;
            var input = format ? _testJsonFormatted : _testJsonSlim;

            // Act
            var result = JsonParser.Parse(input);

            // Assert
            result.Should().BeEquatableTo(expected);
        }
    }
}
