using FluentAssertions;
using FluentSerializer.Json.Configuration;
using FluentSerializer.Json.Services;
using FluentSerializer.Json.Stories.Mavenlink.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace FluentSerializer.Json.Tests
{
    public partial class MavenlinkTests
    {

        [Fact]
        public async Task Serialize()
        {
            // Arrange
            var expected = await File.ReadAllTextAsync("./Serialize.GetProject.json");
            var example = new Request<Project>();
            var profiles = new List<JsonSerializerProfile>
            {
            };
            var sut = new FluentJsonSerializer(profiles);

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
            var example = await File.ReadAllTextAsync("./Deserialize.GetProject.json");
            var profiles = new List<JsonSerializerProfile>
            {
            };
            var sut = new FluentJsonSerializer(profiles);

            // Act
            var result = sut.Deserialize<Project>(example);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
