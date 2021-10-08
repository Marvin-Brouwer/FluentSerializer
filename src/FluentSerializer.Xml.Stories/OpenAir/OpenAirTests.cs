using FluentAssertions;
using FluentSerializer.Xml.Stories.OpenAir.Models;
using FluentSerializer.Xml.Configuration;
using FluentSerializer.Xml.Services;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace FluentSerializer.Xml.Tests
{
    public partial class OpenAirTests
    {

        [Fact]
        public async Task Serialize()
        {
            // Arrange
            var expected = await File.ReadAllTextAsync("./Serialize.Xml");
            var example = new Request<Project>();
            var profiles = new List<XmlSerializerProfile>
            {
            };
            var sut = new FluentXmlSerializer(profiles);

            // Act
            var result = sut.Serialize(example);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Deserialize()
        {
            // Arrange
            var expected = new Response<Project>();
            var example = await File.ReadAllTextAsync("./Deserialize.Xml");
            var profiles = new List<XmlSerializerProfile>
            {
            };
            var sut = new FluentXmlSerializer(profiles);

            // Act
            var result = sut.Deserialize<Project>(example);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
