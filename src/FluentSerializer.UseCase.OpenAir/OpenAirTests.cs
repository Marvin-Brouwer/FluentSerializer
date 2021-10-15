using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Profiles;
using FluentSerializer.UseCase.OpenAir.Models;
using FluentSerializer.UseCase.OpenAir.Models.Response;
using FluentSerializer.Xml.Constants;
using FluentSerializer.Xml.Services;

using Xunit;

namespace FluentSerializer.UseCase.OpenAir
{
    public sealed partial class OpenAirTests
    {
        private readonly IScanList<(Type type, SerializerDirection direction), IClassMap> _mappings;
        private readonly SerializerConfiguration _configuration;

        public OpenAirTests()
        {
            _mappings = ProfileScanner.FindClassMapsInAssembly(typeof(OpenAirTests).Assembly);

            _configuration = ConfigurationConstants.GetDefaultXmlConfiguration();
            _configuration.Encoding = Encoding.UTF8;
        }

        [Fact]
        public async Task Serialize()
        {
            // Arrange
            var expected = await File.ReadAllTextAsync("../../../OpenAirTests.Serialize.Xml");
            var example = ProjectRequestExample;

            var sut = new FluentXmlSerializer(_mappings, _configuration);

            // Act
            var result = sut.Serialize(example);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Deserialize()
        {
            // Arrange
            var expected = RateCardResponseExample;
            var example = await File.ReadAllTextAsync("../../../OpenAirTests.Deserialize.Xml");
            var sut = new FluentXmlSerializer(_mappings, _configuration);

            // Act
            var result = sut.Deserialize<Response<RateCard>>(example);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        private static DateTime CreateDate(string dateString) => DateTime.ParseExact(
            dateString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
    }
}
