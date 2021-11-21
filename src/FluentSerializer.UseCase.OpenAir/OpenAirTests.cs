using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Profiles;
using FluentSerializer.UseCase.OpenAir.Models;
using FluentSerializer.UseCase.OpenAir.Models.Response;
using FluentSerializer.Xml.Configuration;
using FluentSerializer.Xml.Converter.DefaultXml.Converting.Extensions;
using FluentSerializer.Xml.Converting;
using FluentSerializer.Xml.Profiles;
using FluentSerializer.Xml.Services;
using Microsoft.Extensions.ObjectPool;
using Xunit;

namespace FluentSerializer.UseCase.OpenAir
{
    public sealed partial class OpenAirTests
    {
        private readonly IScanList<(Type type, SerializerDirection direction), IClassMap> _mappings;
        private readonly XmlSerializerConfiguration _configuration;

        public OpenAirTests()
        {
            _configuration = XmlSerializerConfiguration.Default;
            _configuration.Encoding = Encoding.UTF8;
            _configuration.DefaultPropertyNamingStrategy = Names.Use.SnakeCase;
            _configuration.DefaultConverters.Add(Converter.For.Xml());

            _mappings = ProfileScanner.FindClassMapsInAssembly<XmlSerializerProfile>(typeof(OpenAirTests).Assembly, _configuration);
        }

        [Fact]
        public async Task Serialize()
        {
            // Arrange
            var expected = await File.ReadAllTextAsync("./OpenAirTests.Serialize.Xml");
            var example = ProjectRequestExample;

            var sut = new RuntimeXmlSerializer(_mappings, _configuration, new DefaultObjectPoolProvider());

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
            var example = await File.ReadAllTextAsync("./OpenAirTests.Deserialize.Xml");
            var sut = new RuntimeXmlSerializer(_mappings, _configuration, new DefaultObjectPoolProvider());

            // Act
            var result = sut.Deserialize<Response<RateCard>>(example);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        private static DateTime CreateDate(string dateString) => DateTime.ParseExact(
            dateString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
    }
}
