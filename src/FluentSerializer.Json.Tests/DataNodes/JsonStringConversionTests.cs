using System;
using System.Text;
using FluentAssertions;
using FluentSerializer.Json.DataNodes;
using Xunit;

namespace FluentSerializer.Json.Tests.DataNodes
{
    public sealed class JsonStringConversionTests

    {
        private readonly JsonObject _testObject;
        private readonly string _testJsonFormatted;
        private readonly string _testJsonSlim;

        public JsonStringConversionTests
()
        {
            _testObject = new JsonObject(
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
            var result = _testObject.ToString(format);

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
            var offset = 0;
            var result = new JsonObject(input.AsSpan(), new StringBuilder(), ref offset);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
