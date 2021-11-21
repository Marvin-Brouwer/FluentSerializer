using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Profiles;
using FluentSerializer.Json.Configuration;
using FluentSerializer.Json.Converter.DefaultJson.Extensions;
using FluentSerializer.Json.Converting;
using FluentSerializer.Json.Profiles;
using FluentSerializer.Json.Services;
using FluentSerializer.UseCase.Mavenlink.Models;
using Microsoft.Extensions.ObjectPool;
using Xunit;

namespace FluentSerializer.UseCase.Mavenlink
{
    public sealed partial class MavenlinkTests
    {
        private readonly IScanList<(Type type, SerializerDirection direction), IClassMap> _mappings;
        private readonly JsonSerializerConfiguration _configuration;

        public MavenlinkTests()
        {
            _configuration = JsonSerializerConfiguration.Default;
            _configuration.FormatOutput = false;
            _configuration.DefaultConverters.Add(Converter.For.Json());

            _mappings = ProfileScanner.FindClassMapsInAssembly<JsonSerializerProfile>(typeof(MavenlinkTests).Assembly, _configuration);
        }

        [Fact]
        public async Task Serialize()
        {
            // Arrange
            var expected = await File.ReadAllTextAsync("./MavenlinkTests.Serialize.json");
            var example = ProjectRequestExample;

            var sut = new RuntimeJsonSerializer(_mappings, _configuration, new DefaultObjectPoolProvider());

            // Act
            var result = sut.Serialize(example);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Deserialize()
        {
            // Arrange
            var expected = UserResponseExample;
            var example = await File.ReadAllTextAsync("./MavenlinkTests.Deserialize.json");

            var sut = new RuntimeJsonSerializer(_mappings, _configuration, new DefaultObjectPoolProvider());

            // Act
            var result = sut.Deserialize<Response<User>>(example);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        private static DateTime CreateDate(string dateString) => DateTime.ParseExact(
            dateString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
    }
}
